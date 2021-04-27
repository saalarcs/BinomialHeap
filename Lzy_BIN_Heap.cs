/*
 * By: Saalar Faisal
 * Building a Lazy Binomial Heap
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAZY_BINOMIAL_HEAP
{
    public class BinomialNode<T>
    {
        public T Item { get; set; }
        public int Degree { get; set; }
        public BinomialNode<T> LeftMostChild { get; set; }
        public BinomialNode<T> RightSibling { get; set; }

        // Constructor

        public BinomialNode(T item)
        {
            Item = item;
            Degree = 0;
            LeftMostChild = null;
            RightSibling = null;
        }
    }

    //--------------------------------------------------------------------------------------

    // Common interface for all non-linear data structures

    public interface IContainer<T>
    {
        void MakeEmpty();  // Reset an instance to empty
        bool Empty();      // Test if an instance is empty
        int Size();        // Return the number of items in an instance
    }

    //--------------------------------------------------------------------------------------

    public interface IBinomialHeap<T> : IContainer<T> where T : IComparable
    {
        void Add(T item);               // Add an item to a binomial heap
        void Remove();                  // Remove the item with the highest priority
        T Front();                      // Return the item with the highest priority
        void Coalesce();                // Combine bionomial tree

        
    }

    //--------------------------------------------------------------------------------------

    // Binomial Heap
    // Implementation:  Leftmost-child, right-sibling

    public class BinomialHeap<T> : IBinomialHeap<T> where T : IComparable
    {
        private BinomialNode<T> [] B;  // Array of Bk values
        private int size;              // Size of the binomial heap
        private T greatestItem;      // Value of largest Node
        private T linkToGreatestItem = default(T);

        // Contructor
        // Time complexity:  O(1)

        public BinomialHeap()
        {
            B = new BinomialNode<T>[15]; // holds 2^15 ~ 32000 values
            size = 0;
            greatestItem = default(T);
        }

        // Add
        // Inserts an item into the binomial heap
        // Time complexity:  O(log n)

        public void Add(T item)
        {        
            // we are storing refernce at head of array
            // if null then instantiate the node at root
            if(B[0] == null)
            {
                // set to it as a reference
                B[0] = new BinomialNode<T>(item);
                // update 
                greatestItem = item;
                size++;
            }

            // If we already have a non-empty array:
            // We have to compare the item to our greatestItem     
            else if (item.CompareTo(greatestItem) > 0)
            {
                // set it as the great Item
                greatestItem = item;
                // the node preceding the greatest item is set to default
                // because greatest is at root
                linkToGreatestItem = default(T);
                // add to the front of array B[0]
                BinomialNode<T> newVal = new BinomialNode<T>(item);
                // Previous Link is set to RS of newVal
                newVal.RightSibling = B[0];
                // add newVal as the new reference
                B[0] = newVal;
                //increment the size
                size++;

            }

            // If its less than the greatestItem
            else
            {
                // if the greatest Item was at root (reference of array)
                if (B[0].Item.Equals(greatestItem))
                {
                    // then rightchild of added tree will be the greatestItem
                    linkToGreatestItem = item;
                }

                // here we add new item to the front of array B[0]
                BinomialNode<T> newVal = new BinomialNode<T>(item);
                // Previous Link is set to RS of newVal
                newVal.RightSibling = B[0];
                // add newVal as the new reference
                B[0] = newVal;
                //increment the size
                size++;
            }
        }


        //Printing Method
        //Returns the array with the counts of item

        public void Print()
        {
            // Our total size
            Console.WriteLine("Total elements in Binomial Heap is {0}.", Size());

            BinomialNode<T> temp;

            // run through entire array
            for (int i=0; i<B.Length; i++)
            {
                int count = 0;
                temp = B[i];

                // if its not null, then count the trees inside
                while(temp != null)
                {
                    count++;
                    temp = temp.RightSibling;
                }

                //display total in each array
                Console.WriteLine("B[{0}] has a size of {1}", i, count);
            }


        }

        // Public Remove
        // Uses RemoveGreatest: removes the greatestItem
        // and Coalesce: Combines into 0 or 1 trees  
        public void Remove()
        {
            RemoveGreatest();
            Coalesce();
        }

        // This lets us know of the values inside the BH
        //-- I used it for testing purpose--
        // Prints out the items in the Binomial Heap
        public void GetItems()
        {
            for (int i = 0; i < B.Length; i++)
            {
                BinomialNode<T> curr = B[i];

                while(curr != null)
                {
                    Console.WriteLine("In B[{0}] there is stored {1}", i, curr.Item);
                    curr = curr.RightSibling;
                }
            }
        }

        // Removes the item with the highest priority from the binomial heap
        // Time complexity:  O(log n)
        private void RemoveGreatest()
        {
            if (!Empty())
            {
                // create pointer for traversal
                BinomialNode<T> temp;

                for (int i = 0; i < B.Length; i++) // loop array
                {
                    // set poointer to the first array
                    temp = B[i];

                    // flag for completion
                    bool done = false;

                    // node to be removed stored here
                    BinomialNode<T> rem;

                    // if array has a tree
                    while (temp != null && !done)
                    {
                        // if the first item/reference of array is the greatest value
                        if (temp.Item.Equals(greatestItem))
                        {
                            // store tree 
                            rem = temp;

                            // remove it as greatestItem
                            greatestItem = default(T);
                            
                            // decease size
                            size--;
                            
                            //set it to its right sibling
                            B[i] = B[i].RightSibling;

                            BinomialNode<T> curr = rem.LeftMostChild; // traverse LeftMostChild

                            while (curr != null) // while its not null
                            {
                                //hold references of leftmostChild
                                BinomialNode<T> hold = curr.RightSibling;
                                // store the nodes in the array we are adding it to
                                curr.RightSibling = B[curr.Degree];
                                // add to front of array
                                B[curr.Degree] = curr;
                                //set next pointer as RS of curr
                                curr = hold;
                            }

                            // done
                            done = true;

                        }

                        // if the Greatest Item is the Right Child of the reference array
                        // we could have duplicate we have to ensure its the correct one
                        if(temp.Item.Equals(linkToGreatestItem) && temp.RightSibling.Item.Equals(greatestItem))
                        {
                            rem = temp.RightSibling;

                            // set pointers to default
                            greatestItem = default(T);
                            linkToGreatestItem = default(T);

                            // decease size
                            size--;

                            // remove link 
                            temp.RightSibling = temp.RightSibling.RightSibling;

                            BinomialNode<T> curr = rem.LeftMostChild;

                            while (curr != null)
                            {
                                //hold references of leftmostChild
                                BinomialNode<T> hold = curr.RightSibling;
                                // store the nodes in the array we are adding it to
                                curr.RightSibling = B[curr.Degree];
                                // add to front of array
                                B[curr.Degree] = curr;
                                //set next pointer as RS of curr
                                curr = hold;
                            }

                            // we're done
                            done = true;
                        }             

                        // traverse
                        temp = temp.RightSibling;
                    }
                }
            }
        }

        // -----Coalesce-----
        // Combines the binomial trees until there is 0 or 1 in each array
        // We are going to loop all array with trees/
        // If it contains more than 1 then, we will combine them.
        // Then we will send them to the next degree array
        // and finally remove the link
        public void Coalesce()
        {
            // traversal node
            BinomialNode<T> curr;
             
            for (int i = 0; i < B.Length; i++) // our array loop
            {
                curr = B[i]; // set node to array

                // if there are more than 1 nodes in an array
                while (curr != null && curr.RightSibling != null)
                {
                    // lets store these nodes first
                    BinomialNode<T> firstNode = curr;
                    BinomialNode<T> secondNode = curr.RightSibling;

                    // we can remove them from the orinal BH
                    B[i] = B[i].RightSibling.RightSibling;

                    // we can remove them from the traversal curr as well
                    curr = curr.RightSibling.RightSibling;

                    // store the BinomialLink
                    BinomialNode<T> mergedTree;

                    // if firstNode is greater
                    if (firstNode.Item.CompareTo(secondNode.Item) >= 0)
                        mergedTree = BinomialLink(secondNode, firstNode);

                    else // secondNode is greater
                        mergedTree = BinomialLink(firstNode, secondNode);

                    //updating our greatest Item
                    if (greatestItem == null)
                        greatestItem = mergedTree.Item;

                    if (mergedTree.Item.CompareTo(greatestItem) > 0)
                        greatestItem = mergedTree.Item;

                    // place it into new array
                    // sending the new node to its new tree
                    mergedTree.RightSibling = B[mergedTree.Degree];
                    B[mergedTree.Degree] = mergedTree;
                }
            }
        }

        // Front
        // Returns the item with the highest priority
        // Time complexity:  O(log n)

        public T Front()
        {
            return greatestItem;
        }    

        // BinomialLink
        // Makes child the leftmost child of root
        // Time complexity:  O(1)

        private BinomialNode<T> BinomialLink(BinomialNode<T> child, BinomialNode<T> root)
        {
            child.RightSibling = root.LeftMostChild;
            root.LeftMostChild = child;
            root.Degree++;
            return root;
        }

        // MakeEmpty
        // Empties the array
        // Time complexity:  O(1)
        public void MakeEmpty()
        {
            B = null;
            size = 0;
        }

        // Empty
        // Returns true is the binomial heap is empty; false otherwise
        // Time complexity:  O(1)
        public bool Empty()
        {
            return size == 0;
        }

        // Size
        // Returns the number of items in the binomial heap
        // Time complexity:  O(1)
        public int Size()
        {
            return size;
        }
    }

    //--------------------------------------------------------------------------------------

    // Used by class BinomailHeap<T>
    // Implements IComparable and overrides ToString (from Object)

    public class PriorityClass : IComparable
    {
        private int priorityValue;
        private char letter;

        public PriorityClass(int priority, char letter)
        {
            this.letter = letter;
            priorityValue = priority;
        }

        public int CompareTo(Object obj)
        {
            PriorityClass other = (PriorityClass)obj;   // Explicit cast
            return priorityValue - other.priorityValue;  // High values have higher priority
        }

        public override string ToString()
        {
            return letter.ToString() + " with priority " + priorityValue;
        }
    }

    //--------------------------------------------------------------------------------------

    // Test for above classes

    public class Test
    {
        public static void Main(string[] args)
        {
            int i;
            Random r = new Random();

            // instantiate Binomial Heap
            BinomialHeap<PriorityClass> BH = new BinomialHeap<PriorityClass>();

            // Add items
            for (i = 0; i < 51; i++)
            {
                BH.Add(new PriorityClass(r.Next(50), (char)('a')));
            }

            // Test 1: Print all inserted and Front
            BH.Print();
            Console.WriteLine();
            Console.WriteLine(BH.Front());
            Console.WriteLine();

            //Test 2: Remove greatestItems
            BH.Remove();
            BH.Print();
            Console.WriteLine();

            //Test 3: Extra testing: Get Items
            BH.GetItems();


            Console.ReadLine();
        }
    }
}

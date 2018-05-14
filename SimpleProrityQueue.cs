using System;
using System.Collections;
using System.Collections.Generic;

namespace Laboratory1 {
    public class SimplePriorityQueue : IEnumerable<IState> {

		#region Fields

		/// <summary>
		/// BinaryHeap containing states
		/// </summary>
		private List<IState> binaryHeap;

		/// <summary>
		/// Comparer
		/// </summary>
		private IComparer<IState> comparer;

		/// <summary>
		/// Dictionary containing states.ID for fast Contains method
		/// </summary>
		private HashSet<string> map;

		#endregion //Fields

		#region Properties

		/// <summary>
		/// Number of states.
		/// </summary>
		public int Count {
            get { return this.binaryHeap.Count; }
        }

		/// <summary>
		/// All states inside PriorityQueue
		/// </summary>
		public IList<IState> Items {
            get { return this.binaryHeap; }
        }

		/// <summary>
		/// Access to specified states by its position in the PriorityQueue
		/// </summary>
		public IState this[int i] {
			get { return this.binaryHeap[i]; }
		}

		#endregion //Properties

		#region Constructor

		/// <summary>
		/// Constructor. class.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		public SimplePriorityQueue(IComparer<IState> comparer) {
            this.binaryHeap = new List<IState>();
            this.comparer = comparer;
            this.map = new HashSet<string>();
        }

		#endregion //Constructor

		#region Methods

		/// <summary>
		/// Determines whether a ProrityQueue object contains the specified element.
		/// </summary>
		/// <returns>true if the ProrityQueue object contains the specified element; otherwise, false.</returns>
		/// <param name="state">The element to locate in the ProrityQueue object.</param>
		public bool Contains(IState state) {
            return this.map.Contains(state.ID);
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<IState> GetEnumerator() {
			foreach (var item in this.binaryHeap) {
				yield return item;
			}
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>
		/// Insert the specified state.
		/// </summary>
		/// <param name="state">State to insert</param>
		public void Insert(IState state) {

            int i = binaryHeap.Count;

            // Add the new item to the bottom of the heap.
            this.binaryHeap.Add(state);

            // Until the new item is greater than its parent item,
            // swap the two
            while (i > 0 && comparer.Compare(this.binaryHeap[(i - 1) / 2], state) > 0) {
                this.binaryHeap[i] = this.binaryHeap[(i - 1) / 2];
                i = (i - 1) / 2;
            }
            // The new index in the list is the appropriate location for
            // the new item.
            this.binaryHeap[i] = state;
            this.map.Add(state.ID);
        }

		/// <summary>
		/// Removes the root.
		/// </summary>
		/// <returns>The root.</returns>
		public IState RemoveRoot() {
            // Throw an exception if the heap is empty.
            if (this.binaryHeap.Count == 0) {
                throw new InvalidOperationException("The heap is empty.");
            }

            // Get the root value's reference.
            IState rootValue = this.binaryHeap[0];

            // Temporarirly store the last item's value.
            IState temporary = this.binaryHeap[this.binaryHeap.Count - 1];

            // Remove the last value.
            this.binaryHeap.RemoveAt(this.binaryHeap.Count - 1);

            // "Bubble up" the heap if there are other items.
            if (this.binaryHeap.Count > 0) {
                // Start at the first index.
                int i = 0;
                // Continue until the halfway point of the heap.
                while (i < this.binaryHeap.Count / 2) {
                    // Continue along with the next left child.
                    int j = (2 * i) + 1;
                    // If j isn't last item AND left child < right child
                    if ((j < this.binaryHeap.Count - 1) && (this.comparer.Compare(this.binaryHeap[j], this.binaryHeap[j + 1]) > 0)) {
                        // Go to the right child
                        j++;
                    }
                    // If the last item is smaller than both siblings at the
                    // current height, break.
                    if (this.comparer.Compare(this.binaryHeap[j], temporary) > 0) {
                        break;
                    }
                    // Move the item at index j up one level.
                    this.binaryHeap[i] = this.binaryHeap[j];
                    // Move index i to the appropriate branch.
                    i = j;
                }
                // Place the temporarily deleted item back at the end of
                // the current branch.
                this.binaryHeap[i] = temporary;
            }

            this.map.Remove(rootValue.ID);
            // Return the original root value.
            return rootValue;
        }

		#endregion //Methods

	}
}
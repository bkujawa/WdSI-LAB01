using System;
using System.Collections;
using System.Collections.Generic;

namespace Laboratory1 {
    /// <summary>
    /// Priority queue, with fast Contains method O(1), and UpdateMethod.
    /// </summary>
    public sealed class PriorityQueue : IEnumerable<IState> {

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
        /// Dictionary containing states.ID and the position in the binaryHeap
        /// </summary>
        private Dictionary<string, int> map;

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
        /// Access to specified states by its ID
        /// </summary>
        public IState this[string ID] {
            get { return this.binaryHeap[this.map[ID]]; }
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
		public PriorityQueue(IComparer<IState> comparer) {
            this.binaryHeap = new List<IState>();
			this.comparer = comparer;
			this.map = new Dictionary<string, int>();
        }

		#endregion //Constructor

		#region Methods

		/// <summary>
		/// Determines whether a ProrityQueue object contains the specified element.
		/// </summary>
		/// <returns>true if the ProrityQueue object contains the specified element; otherwise, false.</returns>
		/// <param name="state">The element to locate in the ProrityQueue object.</param>
		public bool Contains(IState state) {
            return this.map.ContainsKey(state.ID);
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
            this.map.Add(state.ID, i);

			// Until the new item is greater than its parent item,
			// swap the two
            while (i > 0 && comparer.Compare(this.binaryHeap[(i - 1) / 2], state) > 0) {
                this.binaryHeap[i] = this.binaryHeap[(i - 1) / 2];
                this.map[this.binaryHeap[i].ID] = i;

				i = (i - 1) / 2;
			}
			// The new index in the list is the appropriate location for
			// the new item.
            this.binaryHeap[i] = state;
            this.map[state.ID] = i;
            //isValid();
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
                    this.map[this.binaryHeap[i].ID] = i;
					// Move index i to the appropriate branch.
					i = j;
				}
				// Place the temporarily deleted item back at the end of
				// the current branch.
				this.binaryHeap[i] = temporary;
                this.map[temporary.ID] = i;
			}

            this.map.Remove(rootValue.ID);
            //isValid();
			// Return the original root value.
			return rootValue;
		}

        /// <summary>
        /// Updates the item.
        /// </summary>
        /// <param name="stateToUpdate">State to update.</param>
        public void UpdateItem(IState stateToUpdate) {
			// Throw an exception if the heap is empty.
            if (this.binaryHeap.Count == 0) {
				throw new InvalidOperationException("The heap is empty.");
			}

			// Presever the original index so it can be passed along to
			// subscribers of the HeapHasUpdatedItem event.
            int i = this.map[stateToUpdate.ID];

            if (this.binaryHeap.Count > 1) {
				// Move up towards the root if the updated item is smaller
				// than its parent.
                while (i > 0 && (this.comparer.Compare(this.binaryHeap[(i - 1) / 2], stateToUpdate) > 0)) {
					// Swap parent down to current index.
                    this.binaryHeap[i] = this.binaryHeap[(i - 1) / 2];
                    this.map[binaryHeap[i].ID] = i;
					// Advance into parent's spot.
					i = (i - 1) / 2;
				}

				// Move down towards the leaves if the updated item is larger
				// than one of its children. Move along the branch of the
				// smaller of the two child items.
                while (i < this.binaryHeap.Count) {// && (this.comparer.Compare(updatedItem, this.binaryHeap[(i * 2) + 1]) > 0 || this.comparer.Compare(updatedItem, this.binaryHeap[(i * 2) + 2]) > 0)) {

                    if ((i * 2) + 1 < this.binaryHeap.Count) {
                        //At least one children exist
                        if (this.comparer.Compare(stateToUpdate, this.binaryHeap[i * 2 + 1]) > 0) {
                            //Advance to first child.
                            i = (i * 2) + 1;

                            if (i + 1 < this.binaryHeap.Count && this.comparer.Compare(this.binaryHeap[i], this.binaryHeap[i + 1]) > 0) {
                                //The second children exist and advance to second child.
								++i;
                            }
                        }
                        else {
                            //The first children is ok
                            if ((i * 2) + 2 < this.binaryHeap.Count && this.comparer.Compare(stateToUpdate, this.binaryHeap[i * 2 + 2]) > 0) {
								//The second children exist and advance to second child.
								i = i * 2 + 2;
                            }
                            else {
                                //Nothing to do or the second children doesn't exist
                                break;
                            }
                        }
                    }
                    else {
                        //node without children
                        break;
                    }
					// Swap child up to parent index.
                    this.binaryHeap[(i - 1) / 2] = this.binaryHeap[i];
                    this.map[binaryHeap[i].ID] = (i - 1) / 2; 
				}

                this.binaryHeap[i] = stateToUpdate;
                this.map[stateToUpdate.ID] = i;
			}

            //isValid();
		}

		#endregion //Methods

	}
}

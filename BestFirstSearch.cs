﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laboratory1 {
    public abstract class BestFirstSearch {

        private class Comparer : IComparer<IState> {
			public int Compare(IState x, IState y) {
				if (x.H < y.H) {
					return -1;
				}
				else if (x.H > y.H) {
					return 1;
				}
				else {
					return 0;
				}
			}
        }

        #region Protected Fields

        /// <summary>
        /// Zbiór Closed
        /// </summary>
        protected Dictionary<string, IState> closed = null;

        /// <summary>
        /// Stan początkowy.
        /// </summary>
        protected IState initialState = null;

		/// <summary>
		/// Liczba rozwiązań, do odnalezienia przez algorytm
		/// </summary>
		protected int numberOfSolutionsToFind;

		/// <summary>
		/// Zbiór Open
		/// </summary>
		protected SimplePriorityQueue open = null;

        /// <summary>
        /// Lista odnalezionych rozwiązań.
        /// </summary>
        protected List<IState> solutions = null;

        #endregion //end Protected Fields

        #region Properties

        /// <summary>
        /// Zbiór Closed
        /// </summary>
        public IList<IState> Closed {
            get { return this.closed.Values.ToList(); }
        }

        /// <summary>
        /// Zbiór Open
        /// </summary>
        public IList<IState> Open {
            get { return this.open.Items; }
        }

        /// <summary>
        /// Lista odnalezionych rozwiązań.
        /// </summary>
        public IList<IState> Solutions {
            get { return this.solutions; }
        }

        #endregion //end Properties

        #region Constructors

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="initialState">Stan początkowy.</param>
        /// <param name="numberOfSolutionsToFind">Definiuje ile rozwiązań stara się znaleźć algorytm.</param>
        public BestFirstSearch(IState initialState, int numberOfSolutionsToFind = 1) {
            this.closed = new Dictionary<string, IState>();
            this.initialState = initialState;
            this.numberOfSolutionsToFind = numberOfSolutionsToFind;
            this.open = new SimplePriorityQueue(new Comparer());
			this.solutions = new List<IState>();
        }

        #endregion //end Constructors

        #region Protected Methods

        /// <summary>
        /// Metoda powinna zawierać wszelkie niezbędne operacje do zbudowania stanów potomnych.
        /// </summary>
        /// <param name="parent">Stan rodzica.</param>
        protected abstract void buildChildren(IState parent);

        /// <summary>
        /// Zwraca wartość bool mówiąco czy stan podany w parametrze jest rozwiązaniem.
        /// </summary>
        /// <param name="state">Stan do sprawdzenia.</param>
        /// <returns>Wartość bool czy stan jest rozwiązaniem.</returns>
        protected abstract bool isSolution(IState state);

        #endregion //end Private Methods

        #region Public Methods

        /// <summary>
        /// Wykonanie algorytmu BSF.
        /// </summary>
        public void DoSearch() {
            IState currentState = this.initialState;
            while (true) {
                if (isSolution(currentState)) {
                    solutions.Add(currentState);

                    if (solutions.Count >= numberOfSolutionsToFind) {
                        break;
                    }
                }
                else {
                    buildChildren(currentState);

                    foreach (IState child in currentState.Children) {
                        if (!this.closed.ContainsKey(child.ID) && !this.open.Contains(child)) {
                            this.open.Insert(child);
                        }
                    }
                }
				
                this.closed.Add(currentState.ID, currentState);

				if (this.open.Count == 0) {
                    break;
                }
                else {
                    currentState = this.open.RemoveRoot();
                }
            }
        }

        #endregion //end Public Methods
    }
}
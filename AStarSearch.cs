﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Laboratory1 {
    public abstract class AStarSearch {

        private class Comparer : IComparer<IState> {
            public int Compare(IState x, IState y) {
                if (x.F < y.F) {
                    return -1;
                }
                else if (x.F > y.F) {
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
        /// Zbiór Open - stany które mają zostać odwiedzone.
        /// </summary>
        protected PriorityQueue open = null;

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
        public AStarSearch(IState initialState, int numberOfSolutionsToFind = 1) {
            this.closed = new Dictionary<string, IState>();
            this.initialState = initialState;
            this.numberOfSolutionsToFind = numberOfSolutionsToFind;
            this.solutions = new List<IState>();
            this.open = new PriorityQueue(new Comparer());
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
        /// Wykonanie algorytmu A*.
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
                        if (this.closed.ContainsKey(child.ID)) {
                            continue;
                        }

                        if (this.open.Contains(child)) {
                            if (this.open[child.ID].F > child.F) {
                                this.open.UpdateItem(child);
                            }
                        }
                        else {
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
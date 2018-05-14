using System;
using System.Collections.Generic;
using System.Linq;

namespace Laboratory1 {
    public abstract class DepthFirstSearch {

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
		protected Stack<IState> open = null;

		/// <summary>
		/// Kolekcja pozwlajaca na szybkie wyszukanie czy stan występuje w zbiorze open
		/// </summary>
        protected HashSet<string> existInOpen = null;

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
            get { return this.open.ToList(); } 
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
		public DepthFirstSearch(IState initialState, int numberOfSolutionsToFind = 1) {
            this.closed = new Dictionary<string, IState>();
            this.existInOpen = new HashSet<string>();
            this.initialState = initialState;
			this.numberOfSolutionsToFind = numberOfSolutionsToFind;
			this.open = new Stack<IState>();
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
        /// Wykonanie algorytmu Depth First Search.
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

                buildChildren(currentState);
                foreach (IState child in currentState.Children) {

                    if (!this.closed.ContainsKey(child.ID) || !this.existInOpen.Contains(child.ID)) {
						this.open.Push(child);
                        this.existInOpen.Add(child.ID);
                    }
                }

                this.closed.Add(currentState.ID, currentState);

                if (this.open.Count == 0) {
                    break;
                }
                else {
                    currentState = this.open.Pop();
                    this.existInOpen.Remove(currentState.ID);
                }
            }
        }

		#endregion //end Public Methods
	
    }
}
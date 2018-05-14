using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory1
{
    public class PuzzleSearch : BestFirstSearch
    {
        public PuzzleSearch(PuzzleState state) : base(state) { }

        protected override void buildChildren(IState parent)
        {
            PuzzleState state = (PuzzleState)parent;
            List<int> fields = state.Possibilities(); //Wyznacz liste możliwych ruchów dla danego stanu (max 4, min 2)

            foreach (int i in fields) //Dla każdego możliwego ruchu, stwórz stan potomny
                parent.Children.Add(new PuzzleState(state, i));
        }

        protected override bool isSolution(IState state)
        {
            return state.H == 0.0;
        }
    }
}

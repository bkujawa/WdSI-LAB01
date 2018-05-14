using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory1
{
    public class SudokuSearch : BestFirstSearch
    {
        public SudokuSearch(SudokuState state) : base(state) { }

        protected override void buildChildren(IState parent)
        {
            SudokuState state = (SudokuState)parent; //Pobranie stanu rodzicielskiego, z którego tworzymy stany potomne
            List<SudokuField> minMovesFields = new List<SudokuField>(); //Lista pól wymagających najmniej ruchów
            SudokuField minMovesField = state.emptyFields[0]; //Pobranie pierwszego pustego pola ze stanu rodzicielskiego

            //Wyszukiwanie pola wymagającego najmniej ruchów
            for (int i = 1; i < state.emptyFields.Count; ++i)
            {
                if (state.emptyFields[i].availableMoves.Count < minMovesField.availableMoves.Count)
                    minMovesField = state.emptyFields[i];
            }
            minMovesFields.Add(minMovesField);  //Zapisanie pierwszego pola wymagającego najmniej ruchów
        
            //Wyszukiwanie pól wymagających tyle samo ruchów
            foreach (SudokuField i in state.emptyFields)
            {
                if (i != minMovesField)
                {
                    if (i.availableMoves.Count == minMovesField.availableMoves.Count)
                        minMovesFields.Add(i);
                }
            } //Lista minMovesFields zawiera pola wymagające najmniej ruchów

            //Tworzenie nowych stanów potomnych:
            foreach (SudokuField i in minMovesFields) //dla wszystkich pustych pól o minimanej liczbie ruchów
            {
                foreach (int j in i.availableMoves) //dla każdego możliwego ruchu (zawsza taka sama ilość dla konkretnego stanu rodzicielskiego)
                {
                    SudokuState child = new SudokuState(state, j, i.row, i.col); // parent, value, row, col
                    parent.Children.Add(child);
                }
            }
        }

        protected override bool isSolution(IState state)
        {
            return state.H == 0.0;
        }
    }
}

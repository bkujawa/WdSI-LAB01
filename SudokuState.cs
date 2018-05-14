using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory1
{
    public class SudokuState : State
    {
        public const int SMALL_GRID_SIZE = 3; // wielkość wiersza/kolumny małej planszy
        public const int GRID_SIZE = SMALL_GRID_SIZE * SMALL_GRID_SIZE; // wielkość małej planszy, wielkość wiersza/kolumny planszy sudoku
        //public const int GRID = GRID_SIZE * GRID_SIZE; // wielkość planszy sudoku

        private int[,] table;
        //public int[,] Table
        //{
        //    get { return this.table; }
        //    set { this.table = value; }
        //}

        private string id;
        public override string ID
        {
            get { return this.id; }
        }

        //Lista pustych pól
        public List<SudokuField> emptyFields = new List<SudokuField>();

        //Zwraca liste liczb możliwych do wpisania w konkretnym polu(x, y)
        public List<int> GetCandidates(int x, int y)
        {
            List<int> candidates = new List<int>();
            for (int i = 1; i < GRID_SIZE + 1; ++i) //Liczby sudoku, 1-9
            {
                bool collision = false;
                for (int j = 0; j < GRID_SIZE; ++j)
                {
                    //Sprawdzenie czy liczba jest możliwa do użycia w danym polu:
                    if (table[x, j] == i || table[j, y] == i || table[(x - x % 3) + j % 3, (y - y % 3) + j/3] == i) //
                    {
                        //jeżeli nie - sprawdź następną,
                        collision = true;
                        break;
                    }
                }
                if (!collision)
                {
                    //jeżeli tak - dodaj do listy możliwych liczb
                    candidates.Add(i);
                }
            }
            return candidates;
        }

        //Konstruktor podstawowy, tworzący pierwszy stan planszy sudoku
        public SudokuState(string sudokuPattern) : base()
        {
            if (sudokuPattern.Length != GRID_SIZE * GRID_SIZE)
            {
                throw new ArgumentException("SudokuString posiada niewlasciwa dlugosc.");
            }
            
            //Utworzenie id
            this.id = sudokuPattern;
            this.table = new int[GRID_SIZE, GRID_SIZE];
            for (int i = 0; i < GRID_SIZE; ++i)
            {
                for (int j = 0; j < GRID_SIZE; ++j)
                {
                    this.table[i, j] = sudokuPattern[i * GRID_SIZE + j] - 48;
                }
            }
            this.h = ComputeHeuristicGrade();
        }

        //Konstruktor tworzący stany potomne na podstawie rodzica
        //Kopiuje pola stanu rodzicielskiego, wstawia nową wartość i oblicza wartość heurystyczną nowego stanu
        public SudokuState(SudokuState parent, int newValue, int x, int y) : base(parent)
        {
            this.table = new int[GRID_SIZE, GRID_SIZE];
            Array.Copy(parent.table, this.table, this.table.Length);
            this.table[x, y] = newValue;

            StringBuilder builder = new StringBuilder(parent.id);
            builder[x * GRID_SIZE + y] = (char)(newValue + 48);
            this.id = builder.ToString();

            this.h = ComputeHeuristicGrade();
        }

        //Zwraca wartość heurystyczną danego stanu i listę pustych miejsc z możliwymi liczbami do wpisania
        //Im więcej liczb możliwych do wpisania tym większa wartość heurystyczna, gorsza sytuacja
        //Zakłada, że pierwszy stan sudoku był poprawny
        //
        public override double ComputeHeuristicGrade() 
        {
            double heuristicsGrade = 0.0;
            for (int i = 0; i < GRID_SIZE; ++i)
            {
                for (int j = 0; j < GRID_SIZE; ++j)
                {
                    if (table[i, j] == 0)
                    {
                        List<int> candidates = GetCandidates(i, j);
                        heuristicsGrade += candidates.Count;
                        emptyFields.Add(new SudokuField(i, j, candidates));
                    }
                }
            }
            return heuristicsGrade;
        }

        //heurystyka naiwna
        //public override double ComputeHeuristicGrade()
        //{
        //    double heuristicsGrade = 0.0;

        //    for (int i = 0; i < GRID_SIZE; ++i)
        //        for (int j = 0; j < GRID_SIZE; ++j)
        //            if (table[i, j] == 0) heuristicsGrade++;
        //            else if (Collision(i, j)) return Double.PositiveInfinity;
        //    return heuristicsGrade;
        //}

        //bool Collision(int x, int y)
        //{
        //   for (int i = 1; i < GRID_SIZE; ++i)
        //    for (int j = 0; j < GRID_SIZE; ++j)
        //        if (table[x, j] == i || table[j, y] == i || table[(x - x % 3) + j % 3, (y - y % 3) + j / 3] == i) // 
        //            return true;
        //    return false;
        //}

        public void Print()
        {
            string parse = "--------------------";
            for (int i = 0; i < SudokuState.GRID_SIZE; ++i)
            {
                for (int j = 0; j < SudokuState.GRID_SIZE; ++j)
                {
                    if (table[i, j] == 0)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write("{0} ", table[i, j]);
                    }

                    if (j == 2 || j == 5) Console.Write("|");
                }
                Console.WriteLine();
               if (i == 2 || i == 5)
               {                   
                    Console.Write(parse);
                    Console.WriteLine();
               }
            }
        }
    }
}

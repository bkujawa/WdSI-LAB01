using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory1
{
    public class PuzzleState : State
    { 
        private string id;
        private int[] table;

        public const int GRID = 3; //Wielkość kolumny/wiersza stanu
        public const int GRID_SIZE = GRID * GRID; //Wielkość tablicy (tablica jednowymiarowa)
        public const int RANDOM_PUZZLES = 1000;

        public bool MisplacedTilesHeuristics; //Flaga heurystyki (Misplaced Tiles lub Manhattan)
        private int EmptyIndex = 0; //Puste pole

        public int[] Table
        {
            get { return this.table; }
            set { this.table = value; }
        }

        public override string ID
        {
            get { return this.id; }
        }

        //Konstruktor tworzy tablice z ułożonymi puzzlami
        //nastepnie generuje losową tablice na podstawie ułożonej, wykonując tysiąc losowych ruchów
        public PuzzleState(Random random, bool useMisplaced) : base()                                                               
        {
            table = new int[GRID_SIZE];
            table[0] = 0;
            for (int i = 1; i < GRID_SIZE; ++i) //Ułożone puzzle
            {
                table[i] = i;
            }
            GenerateRandomPuzzle(random); //Mieszanie

            for (int i = 0; i < GRID_SIZE; ++i)
            {
                this.id += (char)(table[i] + 48);
            }
            MisplacedTilesHeuristics = useMisplaced;
            this.h = ComputeHeuristicGrade();
        }

        //Konstruktor kopiujący (potrzebny do testowania takich samych plansz dla dwóch różnych heurystyk)
        public PuzzleState(PuzzleState startState, bool useMisplaced) : base()
        {
            table = new int[GRID_SIZE];
            Array.Copy(startState.table, this.table, this.table.Length);
            this.id = String.Copy(startState.id);
            MisplacedTilesHeuristics = useMisplaced;
            this.EmptyIndex = startState.EmptyIndex;
            this.h = ComputeHeuristicGrade();
        }

        //Konstruktor budujący stany potomne
        public PuzzleState(PuzzleState parent, int newValue) : base(parent)
        {
            //Skopiuj dane stanu rodzicielskiego
            this.table = new int[GRID_SIZE];
            Array.Copy(parent.table, this.table, this.table.Length);
            EmptyIndex = parent.EmptyIndex;

            SwapTiles(newValue); //Wykonaj ruch ("stwórz stan potomny")

            //Zaktualizuj ID stanu
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < GRID_SIZE; ++i)
            {
                builder.Append((char)(this.table[i] + 48));
            }
            this.id = builder.ToString();
            this.MisplacedTilesHeuristics = parent.MisplacedTilesHeuristics;
            //Zaktualizuj wartość heurystyczną i głębokość przeszukiwania
            this.h = ComputeHeuristicGrade();
            this.g = parent.g + 1;     
        }

        public override double ComputeHeuristicGrade()
        {
            if (MisplacedTilesHeuristics)
            {
                return MisplacedTiles();
            }
            return Manhattan();
        }

        //Sprawdzenie czy liczba znajduje się na poprawnym miejscu
        public int MisplacedTiles() 
        {
            int MisplacedTiles = 0;
            for (int i = 0; i < GRID_SIZE; ++i)
            {
                if (table[i] != 0 && table[i] != i)
                    ++MisplacedTiles;
            }
            return MisplacedTiles;
        }

        //Sprawdzenie jak daleko znajduje się liczba od jej poprawnego miejsca
        public int Manhattan()
        {
            int manhattanGrade = 0;
            for (int i = 0; i < GRID_SIZE; ++i)
            {
                if (table[i] != 0)
                    manhattanGrade += (Math.Abs((table[i] / GRID) - (i / GRID)) + Math.Abs((table[i] % GRID) - (i % GRID)));
            }
            return manhattanGrade;
        }

        //Metoda zwraca liste możliwych ruchów dla stanu potomnego
        public List<int> Possibilities()
        {
            List<int> fields = new List<int>();
            if (EmptyIndex % GRID != 0)//Możliwy jest ruch "od lewej"
                fields.Add(EmptyIndex - 1);
            if (EmptyIndex + 1 % GRID != 0 && EmptyIndex != GRID_SIZE - 1)//Możliwy jest ruch "od prawej"
                fields.Add(EmptyIndex + 1);
            if (EmptyIndex - GRID >= 0 )//Możliwy jest ruch "od góry"                               
                fields.Add(EmptyIndex - GRID);
            if (EmptyIndex + GRID < GRID_SIZE)//Możliwy jest ruch "od dołu"
                fields.Add(EmptyIndex + GRID);
            return fields;
        }

        //Zamiana pól
        void SwapTiles(int EmptyIndex)
        {
            table[this.EmptyIndex] = table[EmptyIndex];
            table[EmptyIndex] = 0;

            this.EmptyIndex = EmptyIndex;
        }
        
        //Pomieszanie losowo pól
        void GenerateRandomPuzzle(Random random)
        {
            for (int i = 0; i < RANDOM_PUZZLES; ++i)
            {
                List<int> fields = Possibilities();
                int newEmptyIndex = fields[random.Next(0, fields.Count)];
                SwapTiles(newEmptyIndex);
            }
        }

        public void Print()
        {
            for (int i = 0; i < GRID_SIZE; ++i)
            {
                if (i % 3 == 0)
                {
                    Console.WriteLine();
                }
                if (table[i] == 0)
                {
                    Console.Write("  ");
                }
                else
                {
                    Console.Write(table[i] + " ");
                }
            }
            Console.WriteLine();
        }
    }
}

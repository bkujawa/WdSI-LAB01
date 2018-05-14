using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laboratory1 {
    class Program {
        static void Main(string[] args)
        {
            bool puzzle = true;
            bool sudoku = false;

            if (sudoku)
            {
                List<string> sudokuPatterns = new List<string>();
                sudokuPatterns.Add("800030000930007000071520900005010620000050000046080300009076850060100032000040006");
                sudokuPatterns.Add("000000600600700001401005700005900000072140000000000080326000010000006842040002930");
                sudokuPatterns.Add("457682193600000007100000004200000006584716239300000008800000002700000005926835471");
                //sudokuPatterns.Add("000012034000056017000000000000000000480000051270000048000000000350061000760035000"); //not enough memory
                //sudokuPatterns.Add("000700800000040030000009001600500000010030040005001007500200600030080090007000002"); //not enough memory
                //sudokuPatterns.Add("100040002050000090008000300000509000700080003000706000007000500090000040600020001"); //not enough memory

                foreach (string pattern in sudokuPatterns)
                {
                    //Sprawdzenie ilości pustych pól
                    int emptyFieldsCount = 0;
                    for (int i = 0; i < pattern.Length; ++i)
                    {
                        if (pattern[i] == '0')
                            emptyFieldsCount++;
                    }

                    SudokuState startState = new SudokuState(pattern); //Stan startowy
                    SudokuSearch searcher = new SudokuSearch(startState);

                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    searcher.DoSearch();
                    watch.Stop();

                    IState state = searcher.Solutions[0];
                    List<SudokuState> solutionPath = new List<SudokuState>();

                    while (state != null)
                    {
                        solutionPath.Add((SudokuState)state);
                        state = state.Parent;
                    }
                    solutionPath.Reverse();

                    foreach (SudokuState s in solutionPath)
                    {
                        s.Print();
                        Console.WriteLine();
                    }
                    Console.WriteLine("Empty fields count: {0}", emptyFieldsCount);
                    Console.WriteLine("States count in Open: {0}", searcher.Open.Count);
                    Console.WriteLine("States count in Closed: {0}", searcher.Closed.Count);
                    Console.WriteLine("Time: {0}s", watch.ElapsedMilliseconds / 1000.0);

                    Console.ReadKey();
                }
            }

            if (puzzle)
            {
                const int TESTS_NUMBER = 100;
                Console.WriteLine("Creating {0} puzzles.", TESTS_NUMBER);

                PuzzleState[] startStatesMisplaced = new PuzzleState[TESTS_NUMBER];
                PuzzleState[] startStatesManhattan = new PuzzleState[TESTS_NUMBER];
                Random random = new Random();

                for (int i = 0; i < TESTS_NUMBER; ++i) //Tworzenie 100 stanów startowych puzzli
                {
                    startStatesMisplaced[i] = new PuzzleState(random, true);
                    startStatesManhattan[i] = new PuzzleState(startStatesMisplaced[i], false);
                }

                double misplacedTilesTimeSum = 0.0;
                double manhattanTimeSum = 0.0;

                long misplacedTilesOpenSum = 0;
                long misplacedTilesClosedSum = 0;
                long manhattanOpenSum = 0;
                long manhattanClosedSum = 0;

                double misplacedTilesPathLength = 0.0;
                double manhattanPathLength = 0.0;

                Console.WriteLine("Attempting to solve the puzzles using Misplaced Tiles heuristic:");
                for (int i = 0; i < TESTS_NUMBER; ++i)
                {
                    Console.WriteLine("Puzzle no. " + i);
                    PuzzleSearch searcher = new PuzzleSearch(startStatesMisplaced[i]);
                    searcher.DoSearch();
                    IState state = searcher.Solutions[0];
                    List<PuzzleState> solutionPath = new List<PuzzleState>();
                   
                    while (state != null)
                    {
                        solutionPath.Add((PuzzleState)state);
                        state = state.Parent;
                    }
                    solutionPath.Reverse();

                    foreach (PuzzleState s in solutionPath)
                    {
                        s.Print();
                        Console.WriteLine();
                    }

                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    searcher.DoSearch();
                    watch.Stop();

                    misplacedTilesTimeSum += watch.ElapsedMilliseconds;
                    misplacedTilesOpenSum += searcher.Open.Count;
                    misplacedTilesClosedSum += searcher.Closed.Count;
                    misplacedTilesPathLength += searcher.Solutions[0].G;

                    Console.WriteLine("\tNumber of states in Open set: " + searcher.Open.Count);
                    Console.WriteLine("\tNumber of states in Closed set: " + searcher.Closed.Count);
                    Console.WriteLine("\tSolution path length: " + searcher.Solutions[0].G);
                    Console.WriteLine("\tElapsed time: {0}s.", watch.ElapsedMilliseconds / 1000.0);
                }

                Console.WriteLine();
                Console.WriteLine("Attempting to solve the puzzles using Manhattan heuristic: ");
                for (int i = 0; i < TESTS_NUMBER; ++i)
                {
                    Console.WriteLine("Puzzle no. {0}.", i + 1);
                    PuzzleSearch searcher = new PuzzleSearch(startStatesManhattan[i]);

                    System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                    watch.Start();
                    searcher.DoSearch();
                    watch.Stop();

                    manhattanTimeSum += watch.ElapsedMilliseconds;
                    manhattanOpenSum += searcher.Open.Count;
                    manhattanClosedSum += searcher.Closed.Count;
                    manhattanPathLength += searcher.Solutions[0].G;

                    Console.WriteLine("\tNumber of states in Open set: " + searcher.Open.Count);
                    Console.WriteLine("\tNumber of states in Closed set: " + searcher.Closed.Count);
                    Console.WriteLine("\tSolution path length: " + searcher.Solutions[0].G);
                    Console.WriteLine("\tElapsed time: {0}s.", watch.ElapsedMilliseconds / 1000.0);
                }
                Console.WriteLine();

                Console.WriteLine("Misplaced Tiles heuristic: ");
                Console.WriteLine("\tAverage time: {0}s.", misplacedTilesTimeSum / 1000.0 / TESTS_NUMBER);
                Console.WriteLine("\tAverage Open states number: " + misplacedTilesOpenSum / TESTS_NUMBER);
                Console.WriteLine("\tAverage Closed states number: " + misplacedTilesClosedSum / TESTS_NUMBER);
                Console.WriteLine("\tAverage solution path length: " + misplacedTilesPathLength / TESTS_NUMBER);
                Console.WriteLine();

                Console.WriteLine("Manhattan heuristic: ");
                Console.WriteLine("\tAverage time: {0}s.", manhattanTimeSum / 1000.0 / TESTS_NUMBER);
                Console.WriteLine("\tAverage Open states number: " + manhattanOpenSum / TESTS_NUMBER);
                Console.WriteLine("\tAverage Closed states number: " + manhattanClosedSum / TESTS_NUMBER);
                Console.WriteLine("\tAverage solution path length: " + manhattanPathLength / TESTS_NUMBER);
                Console.ReadKey();
            }
        }
    }
}

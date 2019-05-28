using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Domain;
using Pazuru.Mapping;
using Pazuru.Sudoku;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pazuru.Presentation.CLI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Startup startup = new Startup();
            await startup.InitializeAsync();
            IGenericServiceProvider serviceProvider = startup.GetServiceProvider();
            IPuzzleService<SudokuPuzzle> puzzleService = serviceProvider.GetService<IPuzzleService<SudokuPuzzle>>();
            //byte[] bs = Encoding.Default.GetBytes(
            //    "800000000003600000070090200050007000000045700000100030001000068008500010090000400");
            //PuzzleState ps = new PuzzleState(bs);
            //SudokuPuzzle s = new SudokuPuzzle(ps);
            for (int i = 0; i < 1000; i++)
            {
                SudokuPuzzle puzzle = puzzleService.Generate();
                string beforePuzzle = puzzleService.Print(puzzle);
                Console.WriteLine("Sudoku");
                Console.Write(beforePuzzle);
                Stopwatch stopwatch = Stopwatch.StartNew();
                PuzzleSolveDto puzzleSolveDto = puzzleService.Solve(puzzle);
                stopwatch.Stop();

                if (puzzleSolveDto.Success)
                {
                    Console.WriteLine($"Solved in: {stopwatch.ElapsedMilliseconds}ms");
                    string afterPuzzle = puzzleService.Print(puzzle);
                    Console.WriteLine(afterPuzzle);
                    //ConsoleKeyInfo input;
                    //do
                    //{
                    //    Console.Write("\rPrint all puzzle states? [y/n]\n");
                    //    input = Console.ReadKey();
                    //    if (input.KeyChar == 'y')
                    //    {
                    //        foreach (PuzzleState state in puzzleSolveDto.PuzzleStates)
                    //        {
                    //            Console.Write("\n" + state);
                    //        }
                    //        break;
                    //    }

                    //    if (input.KeyChar.Equals('n'))
                    //    {
                    //        Console.WriteLine();
                    //        break;
                    //    }
                    //} while (!input.KeyChar.Equals('y') || !input.KeyChar.Equals('n'));
                }
                else
                {
                    Console.WriteLine("Failed to solve puzzle!");
                }
            }
            
            Console.ReadKey();
        }
    }
}

using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Mapping;
using Pazuru.Sudoku;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Pazuru.Domain;

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
                ConsoleKeyInfo input;
                do
                {
                    Console.Write("\rPrint all puzzle states? [y/n]\n");
                    input = Console.ReadKey();
                    if (input.KeyChar == 'y')
                    {
                        foreach (PuzzleState state in puzzleSolveDto.PuzzleStates)
                        {
                            Console.Write("\n" + state);
                        }
                        Console.WriteLine();
                        break;
                    }

                    if (!input.KeyChar.Equals('n'))
                        continue;
                    Console.WriteLine();
                    break;
                } while (!input.KeyChar.Equals('y') || !input.KeyChar.Equals('n'));
            }
            else
            {
                Console.WriteLine("Failed to solve puzzle!");
            }

            Console.WriteLine("Done. Press any key to quit.");
            Console.ReadKey();
        }
    }
}

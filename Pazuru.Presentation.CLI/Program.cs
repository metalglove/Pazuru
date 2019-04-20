using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;
using Pazuru.Domain;
using Pazuru.Sudoku;
using System;
using System.Threading.Tasks;

namespace Pazuru.Presentation.CLI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Startup startup = new Startup();
            await startup.IntializeAsync();
            IServiceProvider serviceProvider = startup.GetServiceProvider();
            IPuzzleService<SudokuPuzzle> puzzleService = serviceProvider.GetService(typeof(IPuzzleService<SudokuPuzzle>)) as IPuzzleService<SudokuPuzzle>;
            SudokuPuzzle puzzle = puzzleService.Generate();
            string beforePuzzle = puzzleService.Print(puzzle);
            Console.WriteLine("Sudoku");
            Console.Write(beforePuzzle);
            PuzzleSolveDto puzzleSolveDto = puzzleService.Solve(puzzle);
            if (puzzleSolveDto.Success)
            {
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
                        break;
                    }

                    if (input.KeyChar.Equals('n'))
                    {
                        Console.WriteLine();
                        break;
                    }
                } while (!input.KeyChar.Equals('y') || !input.KeyChar.Equals('n'));
            }
            else
            {
                Console.WriteLine("Failed to solve puzzle!");
            }
            Console.ReadKey();
        }
    }
}

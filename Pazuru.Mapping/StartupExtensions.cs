using Microsoft.Extensions.DependencyInjection;
using Pazuru.Application.Interfaces;
using Pazuru.Application.Services;
using Pazuru.Domain;
using Pazuru.Sudoku;

namespace Pazuru.Mapping
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            // TODO: wire up services

            #region Puzzles
            serviceCollection.AddTransient<PuzzleGenerator<SudokuPuzzle>, SudokuGenerator>();
            serviceCollection.AddTransient<PuzzlePrinter<SudokuPuzzle>, SudokuPrinter>();

            serviceCollection.AddTransient<IPuzzleService<SudokuPuzzle>, SudokuPuzzleService>(serviceProvider => 
            {
                PuzzleGenerator<SudokuPuzzle> puzzleGenerator = serviceProvider.GetService<PuzzleGenerator<SudokuPuzzle>>();
                PuzzlePrinter<SudokuPuzzle> puzzlePrinter = serviceProvider.GetService<PuzzlePrinter<SudokuPuzzle>>();
                return new SudokuPuzzleService(puzzleGenerator, puzzlePrinter);
            });

            #endregion
            return serviceCollection;
        }
    }
}

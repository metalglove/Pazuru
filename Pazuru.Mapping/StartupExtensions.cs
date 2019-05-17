using Microsoft.Extensions.DependencyInjection;
using Pazuru.Application.Interfaces;
using Pazuru.Application.Services;
using Pazuru.Domain;
using Pazuru.Hitori;
using Pazuru.Sudoku;
using System;

namespace Pazuru.Mapping
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            // TODO: wire up services

            #region Puzzles
            serviceCollection.AddTransient<PuzzleGenerator<SudokuPuzzle>, SudokuGenerator>();
            serviceCollection.AddTransient<PuzzleGenerator<HitoriPuzzle>, HitoriGenerator>();
            serviceCollection.AddTransient<PuzzlePrinter<SudokuPuzzle>, SudokuPrinter>();
            serviceCollection.AddTransient<PuzzlePrinter<HitoriPuzzle>, HitoriPrinter>();

            serviceCollection.AddTransient<IPuzzleService<SudokuPuzzle>, SudokuPuzzleService>(serviceProvider => 
            {
                PuzzleGenerator<SudokuPuzzle> puzzleGenerator = serviceProvider.GetService<PuzzleGenerator<SudokuPuzzle>>();
                PuzzlePrinter<SudokuPuzzle> puzzlePrinter = serviceProvider.GetService<PuzzlePrinter<SudokuPuzzle>>();
                return new SudokuPuzzleService(puzzleGenerator, puzzlePrinter);
            });

            serviceCollection.AddTransient<IPuzzleService<HitoriPuzzle>, HitoriPuzzleService>(serviceProvider =>
            {
                PuzzleGenerator<HitoriPuzzle> puzzleGenerator = serviceProvider.GetService<PuzzleGenerator<HitoriPuzzle>>();
                PuzzlePrinter<HitoriPuzzle> puzzlePrinter = serviceProvider.GetService<PuzzlePrinter<HitoriPuzzle>>();
                return new HitoriPuzzleService(puzzleGenerator, puzzlePrinter);
            });
            #endregion
            return serviceCollection;
        }

        public static IGenericServiceProvider ToGenericServiceProvider(this IServiceProvider serviceProvider)
        {
            return new GenericServiceProvider(serviceProvider);
        }
    }
}

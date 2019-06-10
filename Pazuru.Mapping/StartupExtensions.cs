using Microsoft.Extensions.DependencyInjection;
using Pazuru.Application.Interfaces;
using Pazuru.Application.Services;
using Pazuru.Domain;
using Pazuru.Sudoku;
using System;
using System.Diagnostics.CodeAnalysis;
using Pazuru.Infrastructure;
using Pazuru.Infrastructure.Services;

namespace Pazuru.Mapping
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        private static readonly HttpClientHandler Client = new HttpClientHandler();

        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<PuzzleGenerator<SudokuPuzzle>, SudokuGenerator>();
            serviceCollection.AddTransient<PuzzlePrinter<SudokuPuzzle>, SudokuPrinter>();
            serviceCollection.AddTransient<IRestServiceConnector, RestServiceConnector>(serviceProvider => new RestServiceConnector(Client));
            serviceCollection.AddTransient<IPuzzleStorageService, PuzzleStorageService>();
            serviceCollection.AddTransient<IPuzzleService<SudokuPuzzle>, SudokuPuzzleService>(serviceProvider => 
            {
                PuzzleGenerator<SudokuPuzzle> puzzleGenerator = serviceProvider.GetService<PuzzleGenerator<SudokuPuzzle>>();
                PuzzlePrinter<SudokuPuzzle> puzzlePrinter = serviceProvider.GetService<PuzzlePrinter<SudokuPuzzle>>();
                return new SudokuPuzzleService(puzzleGenerator, puzzlePrinter);
            });

            return serviceCollection;
        }

        public static IGenericServiceProvider ToGenericServiceProvider(this IServiceProvider serviceProvider)
        {
            return new GenericServiceProvider(serviceProvider);
        }
    }
}

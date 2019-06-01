using Microsoft.Extensions.DependencyInjection;
using Pazuru.Application.Interfaces;
using Pazuru.Application.Services;
using Pazuru.Domain;
using Pazuru.Sudoku;
using System;
using System.Net.Http;
using Pazuru.Infrastructure.Services;

namespace Pazuru.Mapping
{
    public static class StartupExtensions
    {
        private static readonly HttpClient Client = new HttpClient();

        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            // TODO: wire up services


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

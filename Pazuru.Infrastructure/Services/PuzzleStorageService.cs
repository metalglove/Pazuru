using System.Collections.Generic;
using System.Threading.Tasks;
using Pazuru.Application.DTOs;
using Pazuru.Application.Interfaces;

namespace Pazuru.Infrastructure.Services
{
    public sealed class PuzzleStorageService : IPuzzleStorageService
    {
        private readonly IRestServiceConnector _restServiceConnector;

        public PuzzleStorageService(IRestServiceConnector restServiceConnector)
        {
            _restServiceConnector = restServiceConnector;
        }

        public async Task<List<PuzzleDto>> GetPreviouslySolvedPuzzles()
        {
            return await _restServiceConnector.GetAsync<List<PuzzleDto>>("/puzzle/getPreviouslySolvedPuzzles");
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public async Task<SolvedPuzzlesDto> GetPreviouslySolvedPuzzles()
        {
            HalRootObject<PuzzleDtoHal[]> rootObject = await _restServiceConnector.GetAsync<HalRootObject<PuzzleDtoHal[]>>("/puzzles/previouslySolvedPuzzles");
            return new SolvedPuzzlesDto
            {
                Puzzles = rootObject.Data?.ToArray<PuzzleDto>() ?? new PuzzleDto[] { }
            };
        }
        public async Task SavePuzzle(PuzzleDto puzzleDto)
        {
            await _restServiceConnector.PostAsync<HalRootObject<PuzzleDtoHal>, PuzzleDto>("/puzzles/savePuzzle", puzzleDto);
        }

        public class HalRootObject<T>
        {
            [JsonProperty("message")]
            public string Message { get; set; }
            [JsonProperty("success")]
            public bool Success { get; set; }
            [JsonProperty("data")]
            public T Data { get; set; }
            [JsonProperty("_links")]
            public Links Links { get; set; }
        }

        public class PuzzleDtoHal : PuzzleDto
        {
            [JsonProperty("_links")]
            public Links Links { get; set; }
        }

        public class Links
        {
            [JsonProperty("self")]
            public Self Self { get; set; }
        }

        public class Self
        {
            [JsonProperty("href")]
            public string Href { get; set; }
            [JsonProperty("templated")]
            public bool Templated { get; set; }
        }
    }
}

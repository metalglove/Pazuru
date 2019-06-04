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

        public async Task<SolvedPuzzles> GetPreviouslySolvedPuzzles()
        {
            HalRootObject rootObject = await _restServiceConnector.GetAsync<HalRootObject>("/puzzles/previouslySolvedPuzzles");
            return new SolvedPuzzles
            {
                Puzzles = rootObject.Embedded?.Puzzles.ToArray<PuzzleDto>() ?? new PuzzleDto[] { }
            };
        }
        public async Task SavePuzzle(PuzzleDto puzzleDto)
        {
            await _restServiceConnector.PostAsync<PuzzleDto, PuzzleDto>("/puzzles/savePuzzle", puzzleDto);
        }

        internal class HalRootObject
        {
            [JsonProperty("_embedded")]
            public Embedded Embedded { get; set; }
            [JsonProperty("_links")]
            public Links Links { get; set; }
        }

        internal class Embedded
        {
            [JsonProperty("puzzles")]
            public PuzzleDtoHal[] Puzzles { get; set; }
        }

        internal class PuzzleDtoHal : PuzzleDto
        {
            [JsonProperty("_links")]
            public Links Links { get; set; }
        }

        internal class Links
        {
            [JsonProperty("self")]
            public Self Self { get; set; }
        }

        internal class Self
        {
            [JsonProperty("href")]
            public string Href { get; set; }
            [JsonProperty("templated")]
            public bool Templated { get; set; }
        }
    }
}

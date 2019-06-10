using Newtonsoft.Json;

namespace Pazuru.Application.DTOs
{
    public class SolvedPuzzlesDto
    {
        [JsonProperty("puzzles")]
        public PuzzleDto[] Puzzles { get; set; }
    }
}

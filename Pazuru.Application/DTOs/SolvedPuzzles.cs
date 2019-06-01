using Newtonsoft.Json;

namespace Pazuru.Application.DTOs
{
    public class SolvedPuzzles
    {
        [JsonProperty("puzzles")]
        public PuzzleDto[] Puzzles { get; set; }
    }
}

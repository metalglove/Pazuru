using System;
using Newtonsoft.Json;

namespace Pazuru.Application.DTOs
{
    public class PuzzleDto
    {
        [JsonProperty("puzzleId")]
        public int PuzzleId { get; set; }
        [JsonProperty("puzzleType")]
        public string PuzzleType { get; set; }
        [JsonProperty("solvedPuzzle")]
        public string SolvedPuzzle { get; set; }
        [JsonProperty("originalPuzzle")]
        public string OriginalPuzzle { get; set; }
    }
}

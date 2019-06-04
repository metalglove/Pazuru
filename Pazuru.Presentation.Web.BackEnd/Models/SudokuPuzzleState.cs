using Newtonsoft.Json;

namespace Pazuru.Presentation.Web.BackEnd.Models
{
    public class SudokuPuzzleState
    {
        [JsonProperty("puzzleAsString")]
        public string PuzzleAsString { get; set; }
    }
}

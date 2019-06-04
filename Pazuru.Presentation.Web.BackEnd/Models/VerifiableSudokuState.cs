using Newtonsoft.Json;

namespace Pazuru.Presentation.Web.BackEnd.Models
{
    public class VerifiableSudokuState : SudokuPuzzleState
    {
        [JsonProperty("currentPuzzle")]
        public string CurrentPuzzle { get; set; }
    }
}

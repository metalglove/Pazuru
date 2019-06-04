using Newtonsoft.Json;

namespace Pazuru.Presentation.Web.BackEnd.Models
{
    public class VerifiedSudokuState
    {
        [JsonProperty("correctIndexes")]
        public int[] CorrectIndexes { get; set; }
    }
}

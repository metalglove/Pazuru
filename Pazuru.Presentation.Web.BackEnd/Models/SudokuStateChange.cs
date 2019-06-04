using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pazuru.Presentation.Web.BackEnd.Models
{
    public class SudokuStateChange
    {
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("numberAfter")]
        public int NumberAfter { get; set; }
        [JsonProperty("numberBefore")]
        public int NumberBefore { get; set; }
        [JsonProperty("changed")]
        public bool Changed { get; set; }
        [JsonProperty("lastEvent")]
        public bool LastEvent { get; set; }
    }
}

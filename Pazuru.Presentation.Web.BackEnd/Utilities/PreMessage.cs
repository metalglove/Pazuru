using Newtonsoft.Json;

namespace Pazuru.Presentation.Web.BackEnd.Utilities
{
    public class PreMessage
    {
        [JsonProperty("eventName")]
        public string EventName { get; set; }
    }
}

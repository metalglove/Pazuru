using Newtonsoft.Json;

namespace Pazuru.Presentation.Web.BackEnd.Utilities
{
    public class PuzzleMessage<TMessageType> where TMessageType : class
    {
        [JsonProperty("eventName")]
        public string EventName { get; set; }
        [JsonProperty("data")]
        public TMessageType Data { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}

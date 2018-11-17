
using Newtonsoft.Json;

namespace TwitterAccess
{
    public class MediaEntity
    {
        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }       

        [JsonProperty("type")]
        public string MediaType { get; set; }
    }
}

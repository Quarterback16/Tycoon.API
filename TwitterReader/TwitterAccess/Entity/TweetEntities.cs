using Newtonsoft.Json;
using System.Collections.Generic;

namespace TwitterAccess
{
	public class TweetEntities
    {
        [JsonProperty("urls")]
        public List<UrlEntity> UrlList { get; set; }

        [JsonProperty("media")]
        public List<MediaEntity> MediaList { get; set; }
    }
}

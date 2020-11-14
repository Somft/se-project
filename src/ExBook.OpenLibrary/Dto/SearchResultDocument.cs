using Newtonsoft.Json;

namespace ExBook.OpenLibrary.Dto
{
    public class SearchResultDocument
    {
        [JsonProperty("key")]
        public string Key { get; set; } = "";

        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("type")]
        public string Type { get; set; } = "";
    }
}
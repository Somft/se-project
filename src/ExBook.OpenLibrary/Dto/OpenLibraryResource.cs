using Newtonsoft.Json;

namespace ExBook.OpenLibrary.Dto
{
    public class OpenLibraryResource
    {
        [JsonProperty("key")]
        public string Key { get; set; } = "";
    }
}

using Newtonsoft.Json;

namespace ExBook.OpenLibrary.Dto
{
    public class Element
    {
        [JsonProperty("key")]
        public string Key { get; set; } = "";
    }
}

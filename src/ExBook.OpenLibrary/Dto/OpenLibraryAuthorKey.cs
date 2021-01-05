using Newtonsoft.Json;

namespace ExBook.OpenLibrary.Dto
{
    internal class OpenLibraryAuthorKey
    {
        [JsonProperty("author")]
        public Element Author { get; set; } = new Element();

        //[JsonProperty("type")]
        //public Element Type { get; set; } = new Element();
    }
}
using Newtonsoft.Json;

using System.Collections.Generic;

namespace ExBook.OpenLibrary.Dto
{
    public class SearchResult
    {
        [JsonProperty("docs")]
        public List<SearchResultDocument> Docs { get; set; }
    }
}

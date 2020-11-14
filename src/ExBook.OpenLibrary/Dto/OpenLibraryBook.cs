using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace ExBook.OpenLibrary.Dto
{
    public class OpenLibraryBook : OpenLibraryResource
    {

        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("first_publish_date")]
        public DateTime? FirstPublishDate { get; set; }

        [JsonProperty("covers")]
        public List<int> Covers { get; set; } = new List<int>();

        [JsonProperty("subjects")]
        public List<string> Subjects { get; set; } = new List<string>();

        [JsonIgnore]
        public List<OpenLibraryAuthor> Authors { get; set; } = new List<OpenLibraryAuthor>();

        [JsonProperty("authors")]
        internal List<OpenLibraryAuthorKey> AuthorsKeys { get; set; } = new List<OpenLibraryAuthorKey>();
    }
}

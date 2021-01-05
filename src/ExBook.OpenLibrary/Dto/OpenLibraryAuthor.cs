using Newtonsoft.Json;

using System;

namespace ExBook.OpenLibrary.Dto
{
    public class OpenLibraryAuthor : OpenLibraryResource
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

    }
}
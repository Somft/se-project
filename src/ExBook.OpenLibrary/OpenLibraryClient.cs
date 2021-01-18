using ExBook.Extensions;
using ExBook.OpenLibrary.Dto;

using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExBook.OpenLibrary
{
    public class OpenLibraryClient
    {
        private const string baseUrl = "https://openlibrary.org";

        private readonly IHttpClientFactory clientFactory;
        private readonly IMemoryCache cache;

        public OpenLibraryClient(IHttpClientFactory clientFactory, IMemoryCache cache)
        {
            this.clientFactory = clientFactory;
            this.cache = cache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openLibraryId"></param>
        /// <returns></returns>
        public async Task<OpenLibraryBook?> GetBook(string openLibraryId)
        {
            return await this.cache.GetOrCreateAsync($"OL-BOOK-{openLibraryId}", async (_) =>
            {
                try
                {
                    openLibraryId = openLibraryId.Replace("/works/", "");

                    using HttpClient client = this.clientFactory.CreateClient();

                    HttpResponseMessage response = await client.GetAsync($"{baseUrl}/works/{openLibraryId}.json");

                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    string responseString = await response.Content.ReadAsStringAsync();
                    OpenLibraryBook book = JsonConvert.DeserializeObject<OpenLibraryBook>(responseString);
                    book.Authors = (await book.AuthorsKeys.SelectAsync(async (author) => await this.GetAuthor(author.Author.Key)))
                        .Where(a => a != null)
                        .ToList()!;
                    return book;
                }
                catch
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openLibraryId"></param>
        /// <returns></returns>
        public async Task<OpenLibraryAuthor?> GetAuthor(string openLibraryId)
        {
            return await this.cache.GetOrCreateAsync($"OL-AUTHOR-{openLibraryId}", async (_) =>
            {
                try
                {
                    openLibraryId = openLibraryId.Replace("/authors/", "");

                    using HttpClient client = this.clientFactory.CreateClient();

                    HttpResponseMessage response = await client.GetAsync($"{baseUrl}/authors/{openLibraryId}.json");
                    string responseString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<OpenLibraryAuthor>(responseString);
                } 
                catch
                {
                    return null;
                } 
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public async Task<OpenLibraryBook?> SearchBook(string? title, string? author)
        {
            using HttpClient client = this.clientFactory.CreateClient();

            var urlParams = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(title))
            {
                urlParams.Add("title", title);
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                urlParams.Add("author", author);
            }

            IEnumerable<string> query = urlParams.Select(p => $"{p.Key}={p.Value.Replace(" ", "+")}");
            string url = $"{baseUrl}/search.json?{string.Join("&", query)}";


            return await this.cache.GetOrCreateAsync($"OL-SEARCH-{url}", async (_) =>
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    SearchResult result = JsonConvert.DeserializeObject<SearchResult>(await response.Content.ReadAsStringAsync());

                    string? id = result.Docs.Where(doc => doc.Type == "work").FirstOrDefault()?.Key;

                    return id != null ? await this.GetBook(id) : null;
                }
                catch
                {
                    return null;
                }
            });
        }
    }
}

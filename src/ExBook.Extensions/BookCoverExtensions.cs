using System;
using System.Collections.Generic;
using System.Text;

namespace ExBook.Extensions
{
    public static class BookCoverExtensions
    {
        private static string baseUrl = @"https://covers.openlibrary.org/b/id/{0}.jpg";
        public static string? GetOriginalCoverUrl(string coverId)
        {
            if (!string.IsNullOrWhiteSpace(coverId))
            {
                string url = String.Format(baseUrl, coverId);
                return url;
            }
            else
                return null;
        }

        public static string? GetLargeCoverUrl(string coverId)
        {
            if (!string.IsNullOrWhiteSpace(coverId))
            {
                string url = String.Format(baseUrl, coverId + "-L");
                return url;
            }
            else
                return null;
        }

        public static string? GetMediumCoverUrl(string coverId)
        {
            if (!string.IsNullOrWhiteSpace(coverId))
            {
                string url = String.Format(baseUrl, coverId + "-M");
                return url;
            }
            else
                return null;
        }

        public static string? GetSmallCoverUrl(string coverId)
        {
            if (!string.IsNullOrWhiteSpace(coverId))
            {
                string url = String.Format(baseUrl, coverId + "-S");
                return url;
            }
            else
                return null;
        }

    }
}

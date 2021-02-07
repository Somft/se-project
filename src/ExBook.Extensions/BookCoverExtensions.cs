using System;
using System.Collections.Generic;
using System.Text;

namespace ExBook.Extensions
{
    public static class BookCoverExtensions
    {
        private static string baseUrl = @"https://covers.openlibrary.org/b/id/{0}.jpg";
        private static string nullImageUrl = @"https://icons.iconarchive.com/icons/papirus-team/papirus-status/256/image-missing-icon.png";
        public static string? GetOriginalCoverUrl(string coverId)
        {
            if (!string.IsNullOrWhiteSpace(coverId))
            {
                if (int.Parse(coverId) > 0)
                {
                    string url = String.Format(baseUrl, coverId);
                    return url;
                }
                return nullImageUrl;
            }
            else
                return nullImageUrl;
        }

        public static string? GetLargeCoverUrl(string coverId)
        {
            if (!string.IsNullOrWhiteSpace(coverId))
            {
                if (int.Parse(coverId) > 0)
                {
                    string url = String.Format(baseUrl, coverId + "-L");
                    return url;
                }
                return nullImageUrl;
            }
            else
                return nullImageUrl;
        }

        public static string? GetMediumCoverUrl(string coverId)
        {
            if (!string.IsNullOrWhiteSpace(coverId))
            {
                if (int.Parse(coverId) > 0)
                {
                    string url = String.Format(baseUrl, coverId + "-M");
                    return url;
                }
                return nullImageUrl;
            }
            else
                return nullImageUrl;
        }

        public static string? GetSmallCoverUrl(string coverId)
        {
            if (!string.IsNullOrWhiteSpace(coverId))
            {
                if (int.Parse(coverId) > 0)
                {
                    string url = String.Format(baseUrl, coverId + "-S");
                    return url;
                }
                return nullImageUrl;
            }
            else
                return nullImageUrl;
        }

    }
}

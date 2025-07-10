using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace FlapKap.Helpers
{
    public static class HttpRequestHelper
    {
        public static string getLangKey(HttpRequest request)
        {
            var language = getHeaderKey<string>(request, "lang");
            if (string.IsNullOrEmpty(language)) language = getHeaderKey<string>(request, "Accept-Language");
            if (string.IsNullOrEmpty(language)) language = "ar";
            return language;

        }
        public static T? getHeaderKey<T>(HttpRequest request, string key)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T?));
            var stringValue = request.Headers[key].FirstOrDefault();
            var headerKey = string.IsNullOrEmpty(stringValue) || string.IsNullOrWhiteSpace(stringValue) ? default : (T?)converter.ConvertFromString(stringValue);
            return headerKey;

        }

    }
}

using System;
using System.Text.RegularExpressions;

namespace Comb
{
    public class Comb : IComb
    {
        private const string pattern = @"href=""([^""#\+]*)""";
        private Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ICombHttpClient httpClient;

        public Comb(ICombHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public CombResponse Brush(CombRequest request)
        {
            var response = Validate(request, new CombResponse());

            return response;
        }

        private CombResponse Validate(CombRequest request, CombResponse response)
        {
            if(string.IsNullOrEmpty(request.Url) || !Uri.IsWellFormedUriString(request.Url, UriKind.RelativeOrAbsolute))
            {
                response.Errors.Add("Invalid Url");
            }

            return response;
        }
    }
}

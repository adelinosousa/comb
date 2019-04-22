﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Site.Comb
{
    public class Comb : IComb
    {
        private const string pattern = @"(href=\s?""|src=\s?""|file:\s?"")([^""#\+]*)""";
        private Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly ICombHttpClient httpClient;

        public Comb(ICombHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CombResponse> Brush(CombRequest request)
        {
            var response = Validate(request, new CombResponse());
            if (!response.Success) return response;

            var urlDomain = GetUrlDomain(request);
            var link = new CombLink(request.Url, urlDomain);

            await Brush(link, urlDomain, request.Depth);

            response.Result = link;
            return response;
        }

        private CombResponse Validate(CombRequest request, CombResponse response)
        {
            if (string.IsNullOrEmpty(request.Url) || !Uri.IsWellFormedUriString(request.Url, UriKind.RelativeOrAbsolute))
            {
                response.Errors.Add("Invalid Url");
            }

            return response;
        }

        private string GetUrlDomain(CombRequest request)
        {
            return new Uri(request.Url).GetLeftPart(UriPartial.Authority);
        }

        private async Task Brush(CombLink destination, string urlDomain, int maxDepth = 1, int depth = 0)
        {
            if (destination.Combed) return;

            await Task.Run(async () =>
            {
                var htmlContent = await httpClient.FetchHtmlAsync(destination.Value);
                if (string.IsNullOrEmpty(htmlContent)) return;

                var matches = regex.Matches(htmlContent);
                var tasks = new List<Task>();
                depth++;

                foreach (Match match in matches)
                {
                    var link = match.ToLink(urlDomain, destination);

                    if (link.IsDescendent && depth <= maxDepth)
                    {
                        link.SetCombed();
                        tasks.Add(Brush(link, urlDomain, maxDepth, depth));
                    }

                    destination.AddDescendent(link.Value, link);
                }

                Task.WaitAll(tasks.ToArray());
            });
        }
    }
}
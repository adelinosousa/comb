using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Comb
{
    class Program
    {
        const string targetUrl = "https://www.bbc.co.uk/news";

        static HttpClient _httpClient = new HttpClient();

        const string pattern = @"href=""([^""#\+]*)""";
        static Regex rx = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static string urlDomain;

        static void Main(string[] args)
        {
            Stopwatch sw;

            urlDomain = new Uri(targetUrl).GetLeftPart(UriPartial.Authority);

            sw = Stopwatch.StartNew();

            Comb(new Link(targetUrl, urlDomain)).GetAwaiter().GetResult();

            sw.Stop();

            Console.WriteLine($"Elapsed {sw.Elapsed}");

            Console.ReadLine();
        }

        static async Task<string> FetchHtml(string url)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        static async Task Comb(Link destination, int depth = 0)
        {
            destination.Links.TryGetValue(destination.Value, out Link existingLink);

            if (existingLink != null && existingLink.Combed) return;

            await Task.Run(() =>
            {
                var htmlContent = FetchHtml(destination.Value).GetAwaiter().GetResult();

                if (string.IsNullOrEmpty(htmlContent)) return;

                var matches = rx.Matches(htmlContent);

                var tasks = new List<Task>();

                foreach (Match match in matches)
                {
                    var link = match.ToLink(urlDomain, destination);

                    if (link.IsDescendent && link.Type == LinkType.URL && depth < 1)
                    {
                        link.Combed = true;
                        tasks.Add(Comb(link, 1));
                    }

                    destination.AddDescendent(link.Value, link);
                }

                Task.WaitAll(tasks.ToArray());
            });

            Print(destination);
        }

        static void Print(Link link)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}, Links {link.Links.Count}");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(string.Format("| URL ({0,64}) | Type | Descendents ({1,4}) | IsDescendent |", link.Value, link.Descendents.Count));
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
            foreach (var child in link.Descendents.Values.Where(x => x.IsDescendent && x.Type == LinkType.URL))
            {
                Console.WriteLine(string.Format("| {0,70} | {1, 4} | {2,18} | {3,12} |",
                    child.Value.Length > 70 ? child.Value.Substring(child.Value.Length -70) : child.Value, 
                    child.Type, 
                    child.Descendents.Count, 
                    child.IsDescendent));
            }
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------");
        }
    }
}

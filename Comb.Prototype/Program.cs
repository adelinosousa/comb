using Comb.Integration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Comb.Prototype
{
    class Program
    {
        const string targetUrl = "https://www.bbc.co.uk/news";
        private static IServiceProvider serviceProvider;

        static Program()
        {
            serviceProvider = new ServiceCollection().AddComb().BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            var response = serviceProvider.GetService<IComb>().Brush(new CombRequest
            {
                Url = targetUrl
            }).GetAwaiter().GetResult();

            sw.Stop();

            Print(response.Result);

            Console.WriteLine($"Elapsed {sw.Elapsed}");

            Console.ReadLine();
        }

        static void Print(ICombLink link)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}, Links {link.All.Length}");
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            Console.WriteLine(string.Format("| URL ({0,64}) | Type | Descendents ({1,4}) |", link.Value, link.Descendents.Length));
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
            foreach (var child in link.Descendents.Where(x => x.Type == CombLinkType.URL))
            {
                Console.WriteLine(string.Format("| {0,70} | {1, 4} | {2,18} |",
                    child.Value.Length > 70 ? child.Value.Substring(child.Value.Length -70) : child.Value, 
                    child.Type, 
                    child.Descendents.Length));
            }
            Console.WriteLine("------------------------------------------------------------------------------------------------------");
        }
    }
}

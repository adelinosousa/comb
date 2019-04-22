using System.Threading.Tasks;

namespace Site.Comb
{
    public interface ICombHttpClient
    {
        Task<string> FetchHtmlAsync(string url);
    }
}

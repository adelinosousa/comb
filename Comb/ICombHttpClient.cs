using System.Threading.Tasks;

namespace Comb
{
    public interface ICombHttpClient
    {
        Task<string> FetchHtmlAsync(string url);
    }
}

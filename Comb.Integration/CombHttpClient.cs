using System.Net.Http;
using System.Threading.Tasks;

namespace Comb.Integration
{
    public class CombHttpClient : ICombHttpClient
    {
        private readonly HttpClient httpClient;

        public CombHttpClient()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> FetchHtml(string url)
        {
            try
            {
                using (var response = await httpClient.GetAsync(url))
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
    }
}

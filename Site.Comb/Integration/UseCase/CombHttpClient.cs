using System.Net.Http;
using System.Threading.Tasks;

namespace Site.Comb
{
    internal class CombHttpClient : ICombHttpClient
    {
        private readonly HttpClient httpClient;

        public CombHttpClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Android 4.4; Mobile; rv:41.0) Gecko/41.0 Firefox/41.0");
            // Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Mobile Safari/537.36
        }

        public async Task<string> FetchHtmlAsync(string url)
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

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SkiaSharpSimpleCharts.Client.Core
{
    public class HttpRequestExecutor
    {
        public async Task<TResponse> Get<TResponse>(string url)
            where TResponse : class
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            return await HandleResponse<TResponse>(response);
        }

        private static async Task<TResponse> HandleResponse<TResponse>(HttpResponseMessage response) where TResponse : class
        {
            var contentAsString = await response.Content.ReadAsStringAsync();

            try
            {
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TResponse>(contentAsString);
                }
            }
            catch
            {
                throw;
            }

            return null;
        }
    }
}

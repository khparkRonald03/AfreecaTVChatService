using DataModels;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChatClientViewer
{
    /// <summary>
    /// https://docs.microsoft.com/ko-kr/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
    /// Install-Package Microsoft.AspNet.WebApi.Client
    /// </summary>
    public class WebApiCaller
    {
        HttpClient client = new HttpClient();

        public WebApiCaller()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://203.245.0.193/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<JsonModel> GetMatchingModelAsync(JsonModel jsonModel)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync($"Matching/UsersMatching", jsonModel);

                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                jsonModel = await response.Content.ReadAsAsync<JsonModel>();
                return jsonModel;
            }
            catch (Exception e)
            {
                string log = e.Message;
                return null;
            }

        }

        public async Task<string> CheckVersionAsync(string version)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync($"Matching/CheckVersion", version);

                response.EnsureSuccessStatusCode();

                // Deserialize the updated product from the response body.
                var returnMessage = await response.Content.ReadAsAsync<string>();
                return returnMessage;
            }
            catch (Exception e)
            {
                string log = e.Message;
                return string.Empty;
            }

        }

    }
}
using DataModels;
using System;
using System.Collections.Generic;
using System.Net;
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
        static HttpClient client = new HttpClient();


        public static void Get(JsonModel jsonModel)
        {
            RunAsync(jsonModel).GetAwaiter().GetResult();
        }

        //public static JsonModel Post(JsonModel jsonModel)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var url = "http://localhost:11351/";
        //        //var response = await client.PostAsync(url, new StringContent(oJsonObject.ToString(), Encoding.UTF8, "application/json"));
        //        var response = await client.PostAsync<JsonModel>(url, jsonModel);
        //        Console.WriteLine(response);
        //    }
        //}

        public static async Task<Uri> CreateProductAsync(JsonModel jsonModel)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "Matching/UsersMatching", jsonModel);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        static async Task<JsonModel> MatchingJsonModelAsync(JsonModel jsonModel)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"Matching/UsersMatching", jsonModel);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            jsonModel = await response.Content.ReadAsAsync<JsonModel>();
            return jsonModel;
        }

        static async Task RunAsync(JsonModel jsonModel)
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:11351/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                //var url = await CreateProductAsync(jsonModel);

                var result = await MatchingJsonModelAsync(jsonModel);

            }
            catch (Exception e)
            {
                string log = e.Message;
            }

        }

        //public static void Get(JsonModel jsonModel)
        //{
        //    RunAsync(jsonModel).GetAwaiter().GetResult();
        //}

        //public static async Task<Uri> CreateProductAsync(JsonModel jsonModel)
        //{
        //    HttpResponseMessage response = await client.PostAsJsonAsync(
        //        "Matching/UsersMatching", jsonModel);
        //    response.EnsureSuccessStatusCode();

        //    // return URI of the created resource.
        //    return response.Headers.Location;
        //}

        //static async Task<JsonModel> MatchingJsonModelAsync(JsonModel jsonModel)
        //{
        //    HttpResponseMessage response = await client.PutAsJsonAsync(
        //        $"Matching/UsersMatching", jsonModel);
        //    response.EnsureSuccessStatusCode();

        //    // Deserialize the updated product from the response body.
        //    jsonModel = await response.Content.ReadAsAsync<JsonModel>();
        //    return jsonModel;
        //}

        //static async Task RunAsync(JsonModel jsonModel)
        //{
        //    // Update port # in the following line.
        //    client.BaseAddress = new Uri("http://localhost:64195/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    try
        //    {
        //        //var url = await CreateProductAsync(jsonModel);

        //        var result = await MatchingJsonModelAsync(jsonModel);

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    Console.ReadLine();
        //}
    }
}

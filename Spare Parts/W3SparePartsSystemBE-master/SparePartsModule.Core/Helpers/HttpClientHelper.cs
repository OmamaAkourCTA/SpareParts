using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace SparePartsModule.Core.Helpers
{
    public class HttpClientHelper
    {

        public async Task<bool> PostAsync<T>(string uri, T model,string? token)
        {
           

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            if(token != null)
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
           

            var stringPayload = JsonConvert.SerializeObject(model);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
        

            var stringTask = await client.PostAsync(uri, content);
            string apiResponse = await stringTask.Content.ReadAsStringAsync();
            var pares = JsonConvert.DeserializeObject<object>(apiResponse);

            if ((int)stringTask.StatusCode == 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<(bool,string)> PostAsyncForm(string uri, Dictionary<string, string> model, string? token)
        {


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

           
                FormUrlEncodedContent content = new FormUrlEncodedContent(model);
           
           
            //var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");


            var stringTask = await client.PostAsync(uri, content);
            string apiResponse = await stringTask.Content.ReadAsStringAsync();
            var pares = JsonConvert.DeserializeObject<object>(apiResponse);

            if (stringTask.StatusCode == System.Net.HttpStatusCode.Created|| stringTask.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return (true, stringTask.StatusCode.ToString());
            }
            else
            {
                return (false, stringTask.StatusCode+ apiResponse);
            }
        }
        public async Task<T> GetASync<T>(string uri)
        {
        
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            var stringPayload = JsonConvert.SerializeObject("");
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var stringTask = await client.PostAsync(uri, content);
            
            string apiResponse = await stringTask.Content.ReadAsStringAsync();
            var pares = JsonConvert.DeserializeObject<T>(apiResponse);
            return pares;

        }

    }
}

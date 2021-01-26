using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Utilities
{
    public class GetJson
    {
        /// <summary>
        /// GetJsonByURLAsync retrieves a JSON data by URL and returns the data as a List of specified Objects.
        /// </summary>
        /// <param name="URL">The Json URL to contact.</param>
        /// <param name="dateFormat">If there is need to format the DateFormatString of returned Json, enter dateformat here.</param>
        /// <exception cref="HttpRequestException"></exception>
        public static async Task<List<T>> GetJsonByURLAsync<T>(string URL, string dateFormat = "")
        {
            using var client = new HttpClient();
            var json = "";
            try
            {
                HttpResponseMessage response = await client.GetAsync(URL);
                response.EnsureSuccessStatusCode();
                json = response.Content.ReadAsStringAsync().Result;
                await Task.FromResult(json);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            return JsonConvert.DeserializeObject<List<T>>(json, new JsonSerializerSettings
            {
                DateFormatString = dateFormat
            });
        }
    }
}

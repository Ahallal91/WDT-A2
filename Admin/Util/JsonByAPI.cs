using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Util
{
    public static class JsonByAPI
    {
        public async static Task<List<T>> ReturnDeserialisedObject<T>(HttpClient client, string ApiString)
        {
            var response = await client.GetAsync(ApiString);

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<T>>(result);
        }

        public static HttpResponseMessage ReturnResponseEditObject<T>(HttpClient client, string ApiString, T item)
        {
            var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

            var response = client.PutAsync(ApiString, content).Result;

            return response;
        }
    }
}

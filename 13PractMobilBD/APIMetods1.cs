using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _13PractMobilBD
{
    public class APIMetods1
    {
        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert,chain, errors) => true
        });
        private static readonly string _apiBaseUrl = "http://192.168.26.17:7077/";

        public static T Get<T>(string endPoint)
        {
            var response = _httpClient.GetAsync(_apiBaseUrl+endPoint).Result;
            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }
             var content = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<T>(content);
            return data;
        }
        public static string Post<T>(T body, string endPoint)
        {
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8,"application/json");
            var result = _httpClient.PostAsync(_apiBaseUrl + endPoint, content).Result;
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException(result.ReasonPhrase);

            }
            return result.ToString();
        }
        public static string Put<T>(T body,  int id, string endPoint)
        {
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json,Encoding.UTF8,"application/json");
            var result = _httpClient.PutAsync(_apiBaseUrl + endPoint + "/"+id.ToString(), content).Result;
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
            return result.ToString();
        }
        public static string Delete(int id,String endPoint)
        {
            var result = _httpClient.DeleteAsync(_apiBaseUrl + endPoint+ "/"+ id.ToString()).Result;
            return result.ToString();
        }
        // PUT для ClientServices (составной ключ)
        public static string Put<T>(T body, string endpoint)
        {
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // endpoint уже содержит полный путь с параметрами
            var result = _httpClient.PutAsync(_apiBaseUrl + endpoint, content).Result;

            if (!result.IsSuccessStatusCode)
            {
                var errorContent = result.Content.ReadAsStringAsync().Result;
                throw new HttpRequestException($"Ошибка {result.StatusCode}: {errorContent}");
            }

            return result.ToString();
        }

        // DELETE для ClientServices (составной ключ)
        public static string Delete(string endpoint)
        {
            // endpoint уже содержит полный путь с параметрами
            var result = _httpClient.DeleteAsync(_apiBaseUrl + endpoint).Result;

            if (!result.IsSuccessStatusCode)
            {
                var errorContent = result.Content.ReadAsStringAsync().Result;
                throw new HttpRequestException($"Ошибка {result.StatusCode}: {errorContent}");
            }

            return result.ToString();
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _13PractMobilBD
{
    public class APIMetods1
    {
        private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

        private static readonly string _apiBaseUrl = "http://192.168.26.17:7077/";

        // GET: получение данных
        public static T Get<T>(string endPoint)
        {
            try
            {
                var response = _httpClient.GetAsync(_apiBaseUrl + endPoint).Result;

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"Ошибка GET {endPoint}: {response.StatusCode} - {errorContent}");
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<T>(content);
                return data;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Ошибка при GET запросе: {ex.Message}");
            }
        }

        // POST: добавление данных
        public static string Post<T>(T body, string endPoint)
        {
            try
            {
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = _httpClient.PostAsync(_apiBaseUrl + endPoint, content).Result;

                if (!result.IsSuccessStatusCode)
                {
                    var errorContent = result.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"Ошибка POST {endPoint}: {result.StatusCode} - {errorContent}");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Ошибка при POST запросе: {ex.Message}");
            }
        }

        // PUT для обычных сущностей (с одним ID) - например Services, Clients
        public static string PutWithId<T>(T body, int id, string endPoint)
        {
            try
            {
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = _httpClient.PutAsync(_apiBaseUrl + endPoint + "/" + id.ToString(), content).Result;

                if (!result.IsSuccessStatusCode)
                {
                    var errorContent = result.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"Ошибка PUT {endPoint}: {result.StatusCode} - {errorContent}");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Ошибка при PUT запросе: {ex.Message}");
            }
        }

        // PUT для составных ключей (ClientServices) - endpoint уже содержит полный путь
        public static string Put<T>(T body, string fullEndpoint)
        {
            try
            {
                var json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = _httpClient.PutAsync(_apiBaseUrl + fullEndpoint, content).Result;

                if (!result.IsSuccessStatusCode)
                {
                    var errorContent = result.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"Ошибка PUT {fullEndpoint}: {result.StatusCode} - {errorContent}");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Ошибка при PUT запросе: {ex.Message}");
            }
        }

        // DELETE для обычных сущностей (с одним ID)
        public static string DeleteWithId(int id, string endPoint)
        {
            try
            {
                var result = _httpClient.DeleteAsync(_apiBaseUrl + endPoint + "/" + id.ToString()).Result;

                if (!result.IsSuccessStatusCode)
                {
                    var errorContent = result.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"Ошибка DELETE {endPoint}: {result.StatusCode} - {errorContent}");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Ошибка при DELETE запросе: {ex.Message}");
            }
        }

        // DELETE для составных ключей (ClientServices) 
        public static string Delete(string fullEndpoint)
        {
            try
            {
                var result = _httpClient.DeleteAsync(_apiBaseUrl + fullEndpoint).Result;

                if (!result.IsSuccessStatusCode)
                {
                    var errorContent = result.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"Ошибка DELETE {fullEndpoint}: {result.StatusCode} - {errorContent}");
                }

                return result.ToString();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Ошибка при DELETE запросе: {ex.Message}");
            }
        }
    }
}
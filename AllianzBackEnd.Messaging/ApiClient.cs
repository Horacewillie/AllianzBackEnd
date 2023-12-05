using AllianzBackEnd.Domain;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text.Json;

namespace AllianzBackEnd.Messaging
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        static void PrepRequest(HttpContent? content = null)
        {
            if (content is not null)
            {
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/json");
            }
        }

        public async Task<ApiResponse<T>> Post<T>(object data, string url)
            where T : class
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(data));
                PrepRequest(content);


                var response = await _httpClient.PostAsync(url, content);
                return await ApiResponseHandler.Handle<T>(response);
            }
            catch (Exception ex)
            {//log Message
                return ApiResponse<T>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<T>> Post<T>(object data, string url, string apiKey)
           where T : class
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(data));
                PrepRequest(content);
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{apiKey}");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                //if (headers != null)
                //{
                //    foreach (var header in headers)
                //    {
                //        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                //    }
                //}
                var response = await _httpClient.PostAsync(url, content);
                return await ApiResponseHandler.Handle<T>(response);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error Occured while making api to to vendor:::{ex.Message}");
                return ApiResponse<T>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<T>> Get<T>(string url, Dictionary<string, string> data = null)
            where T : class
        {
            try
            {
                string queryString = "?";
                PrepRequest();
                if (data?.Any() == true)
                {
                    foreach (var pair in data)
                    {
                        queryString += $"{pair.Key}={pair.Value}&";
                    }
                    url += queryString;
                }
                var response = await _httpClient.GetAsync(url);
                return await ApiResponseHandler.Handle<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return ApiResponse<T>.Error(ex.Message);
            }

        }

        public async Task<ApiResponse<T>> Get<T>(string url)
            where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                return await ApiResponseHandler.Handle<T>(response);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
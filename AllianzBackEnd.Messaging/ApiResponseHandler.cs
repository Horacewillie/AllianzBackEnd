using AllianzBackEnd.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AllianzBackEnd.Messaging
{
    internal class ApiResponseHandler
    {
        public static async Task<ApiResponse<T>> Handle<T>(HttpResponseMessage response)
            where T : class
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var data = JsonSerializer.Deserialize<T>(jsonResponse);

                    return ApiResponse<T>.Ok(data!);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception from Api Handler:>>>>>{ex.Message}");
                    return ApiResponse<T>.Error(ex.Message);
                }
            }
            else
            {
                return ApiResponse<T>.Error(jsonResponse);
            }
        }
    }
}

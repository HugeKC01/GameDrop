using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GameDrop.Services
{
    public class DateTimeService
    {
        private readonly HttpClient _httpClient;

        public DateTimeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCurrentDateTimeAsync()
            {
                var response = await _httpClient.GetStringAsync("");
                if (string.IsNullOrEmpty(response))
                {
                    throw new Exception("Failed to retrieve datetime from the service.");
                }

                var jsonResponse = JObject.Parse(response);
                var currentDateTimeToken = jsonResponse["currentDateTime"];
                if (currentDateTimeToken == null)
                {
                    throw new Exception("The datetime field is missing in the response.");
                }

                var utcDateTime = DateTime.Parse(currentDateTimeToken.ToString());

                return utcDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            
        }
    }
    

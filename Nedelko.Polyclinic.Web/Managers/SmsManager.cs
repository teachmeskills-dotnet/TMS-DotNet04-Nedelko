using Nedelko.Polyclinic.Interfaces;
using Nedelko.Polyclinic.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nedelko.Polyclinic.Managers
{
    public class SmsManager : ISmsManager
    {
        public async Task<SmsViewModel> SendAsync(string message, string phone)
        {
            var client = new HttpClient();
            var fullPhoneNumber = string.Concat("+37529", phone);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.releans.com/v2/message"),
                Headers =
                {
                    { "Authorization", "Bearer 0f90a1696daa8b8165d5b8cbbb5075a8" },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "sender", "Vicent" },
                    { "mobile", fullPhoneNumber },
                    { "content", message },
                }),
            };

            using var response = await client.SendAsync(request);
            return JsonConvert.DeserializeObject<SmsViewModel>(
                response.Content.ReadAsStringAsync().Result);
        }
    }
}

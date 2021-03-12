﻿using Flurl;
using Flurl.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DBTestCreator_1.Managers
{
    public class SMSManager
    {
        private string url = "https://api.releans.com/v2/";
        public async Task<string> SendSMSAsync(string message)
        {
            var client = new HttpClient();
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
                    { "mobile", "+375291016666" },
                    { "content", message },
                }),
            };
            using (var response = await client.SendAsync(request))
            {
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
    }

}

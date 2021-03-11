using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DBTestCreator_1.Managers
{
    public class SMSManager
    {

        public async Task SendSMSAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://sms77io.p.rapidapi.com/sms"),
                Headers =
                {
                    { "x-rapidapi-key", "05d3f3f247mshaba4980bf68e1fcp1783dejsn0477b0aff069" },
                    { "x-rapidapi-host", "sms77io.p.rapidapi.com" },
                },
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "to", "+375291016666" },
                    { "p", "<REQUIRED>" },
                    { "text", "Dear customer. We want to say thanks for your trust. Use code MINUS10 for 10 % discount on your next order!" },
                }),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(body);
            }
        }
    }

}

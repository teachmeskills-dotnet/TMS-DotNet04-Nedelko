using Flurl;
using Flurl.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBTestCreator_1.Managers
{
    public class SMSManager
    {
        private string url = "https://api.releans.com/v2/";
        public async Task<ResponseSMS> SendSMSAsync(string message)
        {
            var request = url.AppendPathSegment("message")
                .SetQueryParams(new
                {
                    sender = "Vicent",
                    mobile = +375291016666,
                    content = message,
                });
            return await request.WithOAuthBearerToken("0f90a1696daa8b8165d5b8cbbb5075a8").GetJsonAsync();
        }
    }

}

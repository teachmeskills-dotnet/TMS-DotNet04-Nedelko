using Flurl;
using Flurl.Http;
using System.Threading.Tasks;

namespace DBTestCreator_1.Managers
{
    public class SMSManager
    {
        private string url = "https://gateway.sms77.io/api/";
        public async Task<string> SendSMSAsync(string message)
        {
            return await url.AppendPathSegment("sms")
                .SetQueryParams(new
                {
                    p = "bUlBH1Mnfw3j9FTmeu8dzsPK54QrId7oreW7hkTr87OI6ry6JlYvGeVTJ8r7jOwM",
                    to = 375291016666,
                    text = message,
                    from = "sms77.io",
                }).GetStringAsync();
        }
    }

}

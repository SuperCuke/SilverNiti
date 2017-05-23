using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SilverNiti.Core
{
    public class MailgunService
    {
        public MailgunService()
        {
        }

        public async Task SendEmailAsync(string from, string to, string subject, string html)
        {
            using (var client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.mailgun.net/v3/sandbox91fe71bff578470b93a7c8d1cc4794d7.mailgun.org")
            })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("api:key-11ea644f909f52630241eccd5f18f250")));
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("from", from),
                    new KeyValuePair<string, string>("to", to),
                    new KeyValuePair<string, string>("subject", subject),
                    new KeyValuePair<string, string>("html", html)
                });

                await client.PostAsync("messages", content).ConfigureAwait(false);
            }
        }
    }
}

using ParsingPackages.Logging;
using System.Net.Http.Headers;

namespace ParsingPackages.Persistence.AzureDevOps
{
    public class Request
    {
        private readonly AccessConfig accessConfig;
        public Request(AccessConfig accessConfig) 
        {
            this.accessConfig = accessConfig;
        }

        public async Task<string> Send(string apiString)
        {
            string user = accessConfig.userName;
            string personalaccesstoken = accessConfig.accessToken;
            string server = accessConfig.server;
            string collection = accessConfig.collection;
            string requestURL = $"https://{server}/{collection}" + apiString;
            
            string message = requestURL;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", user, personalaccesstoken))));

                    using (HttpResponseMessage response = await client.GetAsync(requestURL))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        message = message + " - " + response.StatusCode;
                        new Logger().infoLogger(message, true, true);
                        return responseBody;
                    }
                }

            }
            catch (Exception ex)
            {
                message = message + " - ошибка выполнения запроса!\n"+
                                 ex.ToString();
                new Logger().errorLogger(message, true, true);
                return string.Empty;
            }
        }
    }
}

using System;
using System.Net.Http;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using JsonClass;
using Newtonsoft.Json;



namespace AI
{
    public class ChatGPU
    {
        private string _apiToken;

        public string ApiToken
        {
            get { return _apiToken; }
            set { _apiToken = value; }
        }
        private const string uribase = "https://api.openai.com/v1/engines/davinci-codex/completions";

        public async Task<string> GetString(string prompt)
        {
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ApiToken);

                var requestBody = new
                {
                    prompt = prompt,
                    max_tokens = 150,
                    temperature = 0.7
                };
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uribase, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);

                return jsonResponse.choice[0].text;
            }
        }


    }
}
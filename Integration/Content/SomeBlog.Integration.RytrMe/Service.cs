using Newtonsoft.Json;
using RestSharp;

namespace SomeBlog.Integration.RytrMe
{
    public class Service
    {
        public Dto.GenerateExecuteDto GenerateExecute(string shortDescription)
        {
            var body = @"{""operation"":""generateExecute"",""params"":{""driveIdFolder"":null,
                ""driveIdFile"":""6147911bf7daa17aa68bd4d5"",""typeId"":""60584cf2c2cdaa000c2a7954"",
                ""toneId"":""60572a649bdd4272b8fe358c"",""languageId"":""60a4c3d60b2ef9000ce86d01"",
                ""contextInputs"":{""SECTION_TOPIC_LABEL"":""" + shortDescription + @"""},
                ""variations"":1}}";
            var response = ExecuteRequest(body);
            var generateExecuteDto = JsonConvert.DeserializeObject<Dto.GenerateExecuteDto>(response.Content);

            return generateExecuteDto;
        }

        public Dto.UsageSummaryDto UsageSummary()
        {
            var body = @"{""operation"":""usageSummary""}";

            var response = ExecuteRequest(body);

            var usageSummary = JsonConvert.DeserializeObject<Dto.UsageSummaryDto>(response.Content);

            return usageSummary;
        }

        private IRestResponse ExecuteRequest(string body)
        {
            var client = new RestClient("https://api.rytr.me/");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("accept", " application/json, text/plain, */*");
            request.AddHeader("accept-encoding", " gzip, deflate, br");
            request.AddHeader("accept-language", " en-US,en;q=0.9,tr;q=0.8,fi;q=0.7");
            request.AddHeader("authentication", " Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjYxMWY4NmU4OWM4NjRhZDhiNzc1ZmMxMSIsImlhdCI6MTYyOTQ1NjEwNX0.LtWAt9bGV5Ve_RSA6Vym2JTYyEZ6mSNg-4p8XDpObFE");
            request.AddHeader("content-type", " application/json");
            request.AddHeader("origin", " https://app.rytr.me");
            request.AddHeader("referer", " https://app.rytr.me/");
            request.AddHeader("sec-ch-ua", " \"Google Chrome\";v=\"93\", \" Not;A Brand\";v=\"99\", \"Chromium\";v=\"93\"");
            request.AddHeader("sec-ch-ua-mobile", " ?0");
            request.AddHeader("sec-ch-ua-platform", " \"Windows\"");
            request.AddHeader("sec-fetch-dest", " empty");
            request.AddHeader("sec-fetch-mode", " cors");
            request.AddHeader("sec-fetch-site", " same-site");
            client.UserAgent = " Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36";

            request.AddParameter(" application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response;
        }
    }
}

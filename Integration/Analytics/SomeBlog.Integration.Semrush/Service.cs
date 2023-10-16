using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SomeBlog.Integration.Semrush.Dto;
using System;
using System.Collections.Generic;

namespace SomeBlog.Integration.Semrush
{
    /// <summary>
    /// aslanserdar63@yandex.com
    /// 636363Sa
    /// 10.10.2021/24.10.2021
    /// </summary>
    public class Service
    {
        private int _userId;
        private string _apiKey;
        public Service(int userId, string apiKey)
        {
            _userId = userId;
            _apiKey = apiKey;
        }

        public Dto.OrganicPositionsResultsDto GetOrganicPositions(string domain, int page, int pageSize = 100, bool isMobile = true)
        {
            var clientToken = new RestClient("https://www.semrush.com/dpa/rpc");
            clientToken.Timeout = -1;
            var requestToken = new RestRequest(Method.POST);

            var requestDto = new Dto.Request.TokenRequestDto()
            {
                Id = 1,
                Method = "token.Get",
            };

            var database = isMobile ? "mobile-tr" : "tr";

            requestDto.Params.Database = database;
            requestDto.Params.ApiKey = _apiKey;
            requestDto.Params.Page = page;
            requestDto.Params.PageSize = pageSize;
            requestDto.Params.ReportType = "organic.positions";
            requestDto.Params.SearchItem = domain;
            requestDto.Params.UserId = _userId;

            var bodyToken = JsonConvert.SerializeObject(requestDto);

            requestToken.AddParameter("application/json", bodyToken, ParameterType.RequestBody);
            IRestResponse responseToken = clientToken.Execute(requestToken);

            Dto.KeywordTokenDto tokenDto = JsonConvert.DeserializeObject<Dto.KeywordTokenDto>(responseToken.Content);

            var client = new RestClient("https://www.semrush.com/dpa/rpc");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            var body = @"[
                      {""id"":2,""jsonrpc"":""2.0"",""method"":""organic.Positions"",""params"":{""token"":""" + tokenDto.result.token + @""",""database"":""" + database + @""",""dateType"":""daily"",""searchItem"":""" + domain + @""",""searchType"":""domain"",""filter"":{},""display"":{""order"":{""field"":""trafficPercent"",""direction"":""desc""},""page"":" + page + @",""pageSize"":" + pageSize + @"},""userId"":" + _userId + @"}},
                      {""id"":3,""jsonrpc"":""2.0"",""method"":""organic.PositionsTotal"",""params"":{""token"":""" + tokenDto.result.token + @""",""database"":""" + database + @""",""dateType"":""daily"",""searchItem"":""" + domain + @""",""searchType"":""domain"",""filter"":{},""display"":{""order"":{""field"":""trafficPercent"",""direction"":""desc""},""page"":" + page + @",""pageSize"":" + pageSize + @"},""userId"":" + _userId + @"}}
                    ]";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var jArray = JArray.Parse(response.Content);

            var organicPositionsResultsDto = JsonConvert.DeserializeObject<Dto.OrganicPositionsResultsDto>(jArray[0].ToString());

            var countDto = JsonConvert.DeserializeObject<Dto.CountResultDto>(jArray[1].ToString());
            organicPositionsResultsDto.Count = countDto.result;

            return organicPositionsResultsDto;
        }

        public void GetCompetitors(string domain, int page, int pageSize = 100, bool isMobile = true)
        {
            var clientToken = new RestClient("https://www.semrush.com/dpa/rpc");
            clientToken.Timeout = -1;
            var requestToken = new RestRequest(Method.POST);

            var requestDto = new Dto.Request.TokenRequestDto()
            {
                Id = 1,
                Method = "token.Get",
            };

            var database = isMobile ? "mobile-tr" : "tr";

            requestDto.Params.Database = database;
            requestDto.Params.ApiKey = _apiKey;
            requestDto.Params.Page = page;
            requestDto.Params.PageSize = pageSize;
            requestDto.Params.ReportType = "organic.competitors";
            requestDto.Params.SearchItem = domain;
            requestDto.Params.UserId = _userId;

            var bodyToken = JsonConvert.SerializeObject(requestDto);

            requestToken.AddParameter("application/json", bodyToken, ParameterType.RequestBody);
            IRestResponse responseToken = clientToken.Execute(requestToken);

            Dto.KeywordTokenDto tokenDto = JsonConvert.DeserializeObject<Dto.KeywordTokenDto>(responseToken.Content);

            var client = new RestClient("https://www.semrush.com/dpa/rpc");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            var body = @"[
                      {""id"":2,""jsonrpc"":""2.0"",""method"":""organic.Competitors"",""params"":{""token"":""" + tokenDto.result.token + @""",""database"":""" + database + @""",""dateType"":""daily"",""searchItem"":""" + domain + @""",""searchType"":""domain"",""filter"":{},""display"":{""order"":{""field"":""competitionLvl"",""direction"":""desc""},""page"":" + page + @",""pageSize"":" + pageSize + @"},""userId"":" + _userId + @"}},
                      {""id"":3,""jsonrpc"":""2.0"",""method"":""organic.CompetitorsTotal"",""params"":{""token"":""" + tokenDto.result.token + @""",""database"":""" + database + @""",""dateType"":""daily"",""searchItem"":""" + domain + @""",""searchType"":""domain"",""filter"":{},""display"":{""order"":{""field"":""competitionLvl"",""direction"":""desc""},""page"":" + page + @",""pageSize"":" + pageSize + @"},""userId"":" + _userId + @"}}
                    ]";

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var jArray = JArray.Parse(response.Content);

            var organicPositionsResultsDto = JsonConvert.DeserializeObject<Dto.OrganicCompetitorsResultDto>(jArray[0].ToString());
            var countDto = JsonConvert.DeserializeObject<Dto.CountResultDto>(jArray[1].ToString());
        }

        public KeywordSummaryDto GetKeywordSummary(string query)
        {
            var keywordSummaryDto = new KeywordSummaryDto();

            var clientToken = new RestClient("https://www.semrush.com/kmt/rpc");
            clientToken.Timeout = -1;
            var requestToken = new RestRequest(Method.POST);
            var bodyToken = @"{""id"":1,""jsonrpc"":""2.0"",""method"":""keywords.GetInfoOverviewN"",""params"":{""user_id"":" + _userId + @",""api_key"":""" + _apiKey + @""",""phrases"":[""" + query + @"""],""database"":""tr"",""date"":""""}}";
            requestToken.AddParameter("application/json", bodyToken, ParameterType.RequestBody);
            IRestResponse responseToken = clientToken.Execute(requestToken);
            KeywordTokenDto tokenModel = JsonConvert.DeserializeObject<KeywordTokenDto>(responseToken.Content);

            var jArray = JArray.Parse(tokenModel.result.keywords[0].ToString());
            List<KeywordVolumeDto> keywordModel = JsonConvert.DeserializeObject<List<KeywordVolumeDto>>(jArray.ToString());

            keywordSummaryDto.KeywordVolumesByCountry = keywordModel;

            var clientVariations = new RestClient("https://www.semrush.com/kmt/rpc");
            clientVariations.Timeout = -1;
            var requestVariations = new RestRequest(Method.POST);
            requestVariations.AddHeader("Authorization", tokenModel.result.token);
            var bodyVariations = @"{""id"":6,""jsonrpc"":""2.0"",""method"":""fts.GetKeywordsOverview"",""params"":{""user_id"":" + _userId + @",""api_key"":""" + _apiKey + @""",""phrase"":""" + query + @""",""device"":0,""currency"":""USD"",""database"":""tr"",""date"":""""}}";
            requestVariations.AddParameter("application/json", bodyVariations, ParameterType.RequestBody);
            IRestResponse responseVariations = clientVariations.Execute(requestVariations);

            KeywordVariationsDto variationsModel = JsonConvert.DeserializeObject<KeywordVariationsDto>(responseVariations.Content);
            keywordSummaryDto.KeywordVariations = variationsModel;

            var clientSerp = new RestClient("https://www.semrush.com/kmt/rpc");
            clientSerp.Timeout = -1;
            var requestSerp = new RestRequest(Method.POST);
            requestSerp.AddHeader("Authorization", tokenModel.result.token);
            var bodySerp = @"{""id"":12,""jsonrpc"":""2.0"",""method"":""serp.GetURLsOverview"",""params"":{""user_id"":" + _userId + @",""api_key"":""" + _apiKey + @""",""phrase"":""" + query + @""",""device"":0,""currency"":""USD"",""database"":""tr"",""date"":""""}}";
            requestSerp.AddParameter("application/json", bodySerp, ParameterType.RequestBody);
            IRestResponse responseSerp = clientSerp.Execute(requestSerp);

            KeywordSerpDto serpModel = JsonConvert.DeserializeObject<KeywordSerpDto>(responseSerp.Content);
            keywordSummaryDto.KeywordSerps = serpModel;

            var clientRelated = new RestClient("https://www.semrush.com/kmt/rpc");
            clientRelated.Timeout = -1;
            var requestRelated = new RestRequest(Method.POST);
            requestRelated.AddHeader("Authorization", tokenModel.result.token);
            var bodyRelated = @"{""id"":10,""jsonrpc"":""2.0"",""method"":""related.GetKeywordsOverview"",""params"":{""user_id"":" + _userId + @",""api_key"":""" + _apiKey + @""",""phrase"":""" + query + @""",""device"":0,""currency"":""USD"",""database"":""tr"",""date"":""""}}";
            requestRelated.AddParameter("application/json", bodyRelated, ParameterType.RequestBody);
            IRestResponse responseRelated = clientRelated.Execute(requestRelated);

            KeywordRelatedDto relatedModel = JsonConvert.DeserializeObject<KeywordRelatedDto>(responseRelated.Content);
            keywordSummaryDto.KeywordRelateds = relatedModel;

            return keywordSummaryDto;
        }
    }
}

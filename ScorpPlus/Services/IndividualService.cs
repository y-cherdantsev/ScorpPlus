using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization.Json;
using ScorpPlus.Dtos;

namespace ScorpPlus.Services
{
    public class IndividualService
    {
        private readonly string _adataToken;
        private readonly RestClient _client;

        public IndividualService(string token)
        {
            _adataToken = token;
            _client = new RestClient("https://api.adata.kz/api/");
        }

        public async Task<IndividualInfoData> GetIndividualData(string iin)
        {
            var token = await SendRequestAsync(iin);
            var individualInfoResult = new IndividualInfoResult {Success = false};
            var leftAttempts = 40;
            while (individualInfoResult.Message != "ready" && --leftAttempts > 0 )
            {
                await Task.Delay(3000);
                individualInfoResult = await GetResultAsync(token);
            }

            return individualInfoResult.Data;
        }


        public async Task<IndividualInfoData> GetIndividualData(long iin)
            => await GetIndividualData(iin.ToString().PadLeft(12, '0'));


        private async Task<string> SendRequestAsync(string iin)
        {
            var request = new RestRequest(Method.GET) {Resource = $"individual/info/{_adataToken}?iinBin={iin}"};
            var response = await _client.ExecuteAsync(request);
            var requestResult = new JsonSerializer().Deserialize<IndividualInfoRequestResult>(response);
            if (requestResult.Success)
                return requestResult.Token;
            throw new Exception("Error while getting token");
        }

        private async Task<IndividualInfoResult> GetResultAsync(string requestToken)
        {
            var request = new RestRequest(Method.GET)
                {Resource = $"individual/info/check/{_adataToken}?token={requestToken}"};
            var response = await _client.ExecuteAsync(request);
            var requestResult = new JsonSerializer().Deserialize<IndividualInfoResult>(response);

            return requestResult;
        }
    }
}
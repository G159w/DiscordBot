using System.Threading.Tasks;
using DiscordBot.GameRequests.Models;
using DiscordBot.RequestFactory;
using DiscordBot.Tools;
using RestSharp;

namespace DiscordBot.GameRequests
{
    public class PubgApi
    {
        private readonly IRequestFactory _requestFactory;
        private readonly RestClient _restClient;

        public PubgApi(IRequestFactory requestFactory)
        {
            _requestFactory = requestFactory;
            _restClient = new RestClient("https://api.playbattlegrounds.com/shards/pc-eu");
        }

        public async Task<FortniteStatsResponse> GetStatsFromApi(string gameUsername)
        {
            var request = _requestFactory.GenerateRequest($"player/{gameUsername}", Method.GET, "pubg");
            var fortniteUser = _restClient.Execute<FortniteResponse>(request);

            if (fortniteUser.StatusCode.IsStatusOk())
            {
                var apiResponse = fortniteUser.Data.ConvertFromApiResponse();
                return apiResponse;
            }
            
            return null;
        }
    }
}
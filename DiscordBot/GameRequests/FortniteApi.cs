using System.Threading.Tasks;
using DiscordBot.GameRequests.Models;
using DiscordBot.RequestFactory;
using DiscordBot.Tools;
using RestSharp;

namespace DiscordBot.GameRequests
{
    public class FortniteApi
    {
        private readonly IRequestFactory _requestFactory;
        private readonly RestClient _restClient;

        public FortniteApi(IRequestFactory requestFactory)
        {
            _requestFactory = requestFactory;
            _restClient = new RestClient("https://api.fortnitetracker.com");
        }

        public async Task<FortniteStatsResponse> GetStatsFromApi(string gameUsername)
        {
            var request = _requestFactory.GenerateRequest($"v1/profile/pc/{gameUsername}", Method.GET, "fortnite");
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


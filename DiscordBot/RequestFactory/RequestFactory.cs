using System;
using RestSharp;

namespace DiscordBot.RequestFactory
{
    public class RequestFactory: IRequestFactory
    {
        private const string LolApiKey = "<KEY>";
        private const string FortniteApiKey = "<KEY>";
        private const string PubgApiKey = "<KEY>";

        public RestRequest GenerateRequest(string url, Method method, string shortGameName)
        {
            var request = new RestRequest(url, method);
            switch (shortGameName.ToLowerInvariant())
            {
                case "lol":
                    request.AddParameter("api_key", LolApiKey);
                    break;
                case "fortnite":
                    request.AddHeader("TRN-Api-Key", FortniteApiKey);
                    break;
                case "pubg":
                    request.AddHeader("Authorization", $"Bearer {PubgApiKey}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shortGameName), shortGameName, null);
            }

            return request;
        }
    }
}
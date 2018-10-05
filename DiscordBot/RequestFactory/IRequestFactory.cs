using RestSharp;

namespace DiscordBot.RequestFactory
{
    public interface IRequestFactory
    {
        RestRequest GenerateRequest(string url, Method method, string shortGameName);
    }
}
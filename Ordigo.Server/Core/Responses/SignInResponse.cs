using Newtonsoft.Json;

namespace Ordigo.Server.Core.Responses
{
    public sealed class SignInResponse
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        public SignInResponse(string username, string accessToken)
        {
            Username = username;
            AccessToken = accessToken;
        }
    }
}

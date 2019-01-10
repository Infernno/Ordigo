using Newtonsoft.Json;

namespace Ordigo.Server.Core.Responses
{
    public sealed class SignUpResponse
    {
        [JsonProperty("is_successful")]
        public bool IsSuccessful { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        public SignUpResponse(bool isSuccessful, string username)
        {
            IsSuccessful = isSuccessful;
            Username = username;
        }
    }
}

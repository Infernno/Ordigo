using Newtonsoft.Json;

namespace Ordigo.Server.Core.Validation
{
    public class ValidationError
    {
        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        [JsonProperty("message")]
        public string Message { get; }

        public ValidationError(string field, string message)
        {
            if (!string.IsNullOrEmpty(field))
                Field = field;

            Message = message;
        }
    }
}

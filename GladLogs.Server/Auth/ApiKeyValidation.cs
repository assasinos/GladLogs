using GladLogs.Server.Consts;
using GladLogs.Server.Middleware;
using Microsoft.VisualBasic;

namespace GladLogs.Server.Auth
{
    public class ApiKeyValidation :IApiKeyValidation
    {
        private readonly IConfiguration _configuration;
        public ApiKeyValidation(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool IsValidApiKey(string userApiKey)
        {
            if (string.IsNullOrWhiteSpace(userApiKey))
                return false;
            string? apiKey = _configuration.GetValue<string>(ApiKeyConsts.ApiKey);
            if (apiKey == null || apiKey != userApiKey)
                return false;
            return true;
        }
    }
}

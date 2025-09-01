using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Dotnet.Crm.Domain.SeedWork.HTTP
{
    public sealed class Request(HttpClient httpClient, ILogger<Request> logger) : IRequest
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<Request> _logger = logger;

        public async Task<HttpResponseMessage> GetAsync(string server, string route, Authentication authentication, string? parameters = null)
        {
            try
            {
                AddHeaders(authentication);

                return await _httpClient.GetAsync($"{server}/{route}?{parameters}");
            }
            catch (Exception e)
            {
                var exceptionObject = new { Exception = e, RequestParameters = new { server, route, parameters } };

                _logger.LogError(JsonConvert.SerializeObject(exceptionObject));
                return new HttpResponseMessage();
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string server, string route, Authentication authentication, object? body = default)
        {
            try
            {
                AddHeaders(authentication);

                HttpContent bodyContent;

                if (body != null)
                {
                    string jsonContent = JsonConvert.SerializeObject(body);
                    bodyContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }
                else
                {
                    bodyContent = new StringContent("{}", Encoding.UTF8, "application/json");
                }

                return await _httpClient.PostAsync($"{server}/{route}", bodyContent);
            }
            catch (Exception e)
            {
                var exceptionObject = new { Exception = e, RequestParameters = new { server, route, body } };

                _logger.LogError(JsonConvert.SerializeObject(exceptionObject));
                return new HttpResponseMessage();
            }
        }

        public async Task<HttpResponseMessage> PutAsync(string server, string route, Authentication authentication, object body)
        {
            try
            {
                AddHeaders(authentication);

                HttpContent bodyContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

                return await _httpClient.PutAsync($"{server}/{route}", bodyContent);
            }
            catch (Exception e)
            {

                var exceptionObject = new { Exception = e, RequestParameters = new { server, route, body } };

                _logger.LogError(JsonConvert.SerializeObject(exceptionObject));
                return new HttpResponseMessage();
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string server, string route, Authentication authentication, string? parameters = null)
        {
            try
            {
                AddHeaders(authentication);

                return await _httpClient.DeleteAsync($"{server}/{route}" + (string.IsNullOrEmpty(parameters) ? "" : $"?{parameters}"));
            }
            catch (Exception e)
            {
                var exceptionObject = new { Exception = e, RequestParameters = new { server, route, parameters } };

                _logger.LogError(JsonConvert.SerializeObject(exceptionObject));
                return new HttpResponseMessage();
            }
        }
        private void AddHeaders(Authentication authentication)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(authentication.AuthorizationOrdinaryToken))
                _httpClient.DefaultRequestHeaders.Add("Authorization", authentication.AuthorizationOrdinaryToken);

            if (!string.IsNullOrEmpty(authentication.GatewayToken))
                _httpClient.DefaultRequestHeaders.Add("Gateway-Authentication", authentication.GatewayToken);
        }
    }
}

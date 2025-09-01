namespace Dotnet.Crm.Domain.SeedWork.HTTP
{
    public interface IRequest
    {
        Task<HttpResponseMessage> GetAsync(string server, string route, Authentication authentication, string? parameters = null);
        Task<HttpResponseMessage> PostAsync(string server, string route, Authentication authentication, object? body = default);
        Task<HttpResponseMessage> PutAsync(string server, string route, Authentication authentication, object body);
        Task<HttpResponseMessage> DeleteAsync(string server, string route, Authentication authentication, string? parameters = null);
    }
}

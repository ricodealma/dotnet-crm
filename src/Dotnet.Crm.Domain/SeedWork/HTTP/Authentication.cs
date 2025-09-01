namespace Dotnet.Crm.Domain.SeedWork.HTTP
{
    public sealed class Authentication : IAuthentication
    {
        public string GatewayToken { get; set; } = string.Empty;
        public string AuthorizationOrdinaryToken { get; set; } = string.Empty;
    }
}

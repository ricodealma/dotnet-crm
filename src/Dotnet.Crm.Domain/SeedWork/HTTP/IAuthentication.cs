namespace Dotnet.Crm.Domain.SeedWork.HTTP
{
    public interface IAuthentication
    {
        string GatewayToken { get; set; }
        string AuthorizationOrdinaryToken { get; set; }
    }
}

namespace Dotnet.Crm.Infra.External
{
    public interface IDistributedMemoryCacheDAO
    {
        public void SetValue(string key, string value, TimeSpan expiration);
        public bool TryGetValue<T>(string key, out T? value);
    }
}

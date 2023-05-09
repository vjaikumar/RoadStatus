namespace RoadStatusService
{
    public interface ITflClient
    {
        Task<HttpResponseMessage> GetAsync(string Url);
    }
}
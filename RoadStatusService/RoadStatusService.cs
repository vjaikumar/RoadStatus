using System.Net;
using System.Text.Json;

namespace RoadStatusService
{
    public  class RoadStatusService
    {
        private readonly ITflClient _tflClient;
        private readonly string _appId;
        private readonly string _appKey;
        private const string _apiUrl = "https://api.tfl.gov.uk/Road/";

        public RoadStatusService(ITflClient tflClient, string appId, string appKey)
        {
            _tflClient = tflClient;
            _appId = appId;
            _appKey = appKey;
        }

        public async Task<(int ErrorCode, RoadStatus RoadStatus)> GetRoadStatusAsync(string roadId)
        {
            var url = $"{_apiUrl}{roadId}?app_id={_appId}&app_key={_appKey}";
            var response = await _tflClient.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNamingPolicy= JsonNamingPolicy.CamelCase;
                var roadStatus = JsonSerializer.Deserialize<RoadStatus[]>(responseJson, options);
                return (ErrorCode: 0, RoadStatus: roadStatus.FirstOrDefault());
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return (ErrorCode: 1, RoadStatus: null);
            }
            else
            {
                throw new ApplicationException("unoknown error while making a TFL API request");
            }
        }
    }
}

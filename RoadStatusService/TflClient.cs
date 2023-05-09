using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadStatusService
{
    public class TflClient : ITflClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public async Task<HttpResponseMessage> GetAsync(string Url)
        {
            return await _httpClient.GetAsync(Url);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace RideMate.Services
{
   public class RoutingService
    {
        private readonly HttpClient _client;

        public RoutingService(HttpClient client)
        {
            _client = client;
        }

        
        public async Task<string> TestApiAsync()
        {
            return await _client.GetStringAsync("https://api.openrouteservice.org/");
        }
    }
}

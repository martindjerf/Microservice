using Microservice.Service.Interfaces;
using Microservice.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microservice.Service.Clients
{
    public class HttpLocationClient : IHttpLocationClient
    {
        public string URL { get; set; } 

        public HttpLocationClient(string url)
        {
            URL = url;
        }

        public async Task<LocationRecord> GetLatestForMemberAsync(Guid memberId)
        {
            LocationRecord locationRecord = null;

            using(HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(URL);

                httpClient.DefaultRequestHeaders.Clear();

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.GetAsync(string.Format("api/locations/{0}/latest", memberId));

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    locationRecord = JsonConvert.DeserializeObject<LocationRecord>(json);
                }
            }

            return locationRecord;
        }
    }
}

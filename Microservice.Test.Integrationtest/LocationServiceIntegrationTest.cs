using Microservice.LocationService;
using Microservice.LocationService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Microservice.Test.Integrationtest
{
    public class LocationServiceIntegrationTest
    {
        private readonly TestServer testServer;
        private readonly HttpClient testClient;
        private readonly LocationRecord locationRecord;
        public LocationServiceIntegrationTest()
        {
            testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            testClient = testServer.CreateClient();
            testClient.BaseAddress = new Uri("http://localhost/");

            locationRecord = new LocationRecord()
            {
                ID = Guid.NewGuid(),
                Altitude = 1200,
                Latitude = 54.12f,
                Longitude = 12.31f,
                MemberID = Guid.NewGuid(),
                Timestamp = 123132123131
            };
        }

        [Fact]
        public async void PostLocationAndGetIt()
        {
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(locationRecord), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await testClient.PostAsync("api/locations/" + locationRecord.MemberID.ToString(), stringContent);

            response.EnsureSuccessStatusCode();

            var getUri = response.Headers.Location.ToString();

            var fetchedLocationRecordRaw = await testClient.GetAsync(getUri);

            var recordraw = fetchedLocationRecordRaw.Content.ReadAsStringAsync();

            var record = JsonConvert.DeserializeObject<LocationRecord>(recordraw.Result);

            Assert.Equal(locationRecord.ID, record.ID);
            
        }

    }
}

using Microservice.Service;
using Microservice.Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Microservice.Test.IntegrationTests
{
    public class SimpleIntegrationTests
    {
        private readonly TestServer testServer;
        private readonly HttpClient testClient;

        private readonly TestServer testServerLocation;
        private readonly HttpClient testClientLocation;

        private readonly Team teamZombie;
        private readonly Member memberZombie;
        private readonly LocationRecord locationRecord;

        public SimpleIntegrationTests()
        {
            testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            testClient = testServer.CreateClient();
            testClient.BaseAddress = new Uri("http://localhost/");

            testServerLocation = new TestServer(new WebHostBuilder().UseStartup<Microservice.LocationService.Startup>());
            testClientLocation = testServerLocation.CreateClient();

            testClient.BaseAddress = new Uri("http://localhost:65390/");

            teamZombie = new Team()
            {
                ID = Guid.NewGuid(),
                Name = "Zombie Team"
            };
            memberZombie = new Member()
            {
                FirstName = "Rob",
                LastName = "Zombie",
                ID = Guid.NewGuid()
            };
            locationRecord = new LocationRecord()
            {
                ID = Guid.NewGuid(),
                Altitude = 12.0f,
                Latitude = 10.0f,
                Longitude = 10.0f,
                MemberID = memberZombie.ID,
                Timestamp = 0
            };
            
        }

        //[Fact]
        //public async void TestTeamPostAndGet()
        //{
        //    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(teamZombie), Encoding.UTF8, "application/json");

        //    HttpResponseMessage postResponse = await testClient.PostAsync("api/teams", stringContent);

        //    postResponse.EnsureSuccessStatusCode();

        //    var getResponse = await testClient.GetAsync("api/teams");
        //    getResponse.EnsureSuccessStatusCode();

        //    string raw = await getResponse.Content.ReadAsStringAsync();

        //    List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(raw);

        //    //Assert.Single(teams);
        //    Assert.Equal("Zombie Team", teams[0].Name);
        //    Assert.Equal(teamZombie.ID, teams[0].ID);

        //}

        [Fact]
        public async void TestPostTeamMemberAndLocationAndFetchIt()
        {
            StringContent stringContent = SerializeContent(teamZombie);
            HttpResponseMessage postResponse = await testClient.PostAsync("api/teams", stringContent);

            postResponse.EnsureSuccessStatusCode();

            var getResponse = await testClient.GetAsync("api/teams");
            getResponse.EnsureSuccessStatusCode();

            string raw = await getResponse.Content.ReadAsStringAsync();

            List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(raw);

            Assert.Single(teams);
            Assert.Equal("Zombie Team", teams[0].Name);
            Assert.Equal(teamZombie.ID, teams[0].ID);

            stringContent = SerializeContent(memberZombie);

            postResponse = await testClient.PostAsync(string.Format("api/teams/{0}/members", teams[0].ID), stringContent);
            postResponse.EnsureSuccessStatusCode();

            getResponse = await testClient.GetAsync(string.Format("api/teams/{0}/members/{1}", teams[0].ID, memberZombie.ID));
            getResponse.EnsureSuccessStatusCode();

            raw = await getResponse.Content.ReadAsStringAsync();

            var member = JsonConvert.DeserializeObject<Member>(raw);

            Assert.Equal(memberZombie.ID, member.ID);
            Assert.Equal(memberZombie.FirstName, member.FirstName);
            Assert.Equal(memberZombie.LastName, member.LastName);

            stringContent = SerializeContent(locationRecord);
            
            postResponse = await testClientLocation.PostAsync(string.Format("api/locations/{0}", member.ID), stringContent);
            postResponse.EnsureSuccessStatusCode();


            getResponse = await testClientLocation.GetAsync(string.Format("api/locations/{0}/{1}", member.ID, locationRecord.ID));
            getResponse.EnsureSuccessStatusCode();

            raw = await getResponse.Content.ReadAsStringAsync();
            var location = JsonConvert.DeserializeObject<LocationRecord>(raw);

            Assert.Equal(locationRecord.ID, location.ID);
            Assert.Equal(locationRecord.MemberID, location.MemberID);
            Assert.Equal(locationRecord.Altitude, location.Altitude);
            Assert.Equal(locationRecord.Longitude, location.Longitude);
            Assert.Equal(locationRecord.Latitude, location.Latitude);



        }

        private StringContent SerializeContent(object objectToSerialize)
        {
            var obj = new object();

            if (objectToSerialize is Team) obj = (Team)objectToSerialize;  
            if (objectToSerialize is Member) obj = (Member)objectToSerialize;
            if (objectToSerialize is LocationRecord) obj = (LocationRecord)objectToSerialize;

            StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            return content;
        }
    }
}

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

        private readonly Team teamZombie;

        public SimpleIntegrationTests()
        {
            testServer = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            testClient = testServer.CreateClient();
            testClient.BaseAddress = new Uri("http://localhost/");

            teamZombie = new Team()
            {
                ID = Guid.NewGuid(),
                Name = "Zombie Team"
            };
        }

        [Fact]
        public async void TestTeamPostAndGet()
        {
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(teamZombie), Encoding.UTF8, "application/json");

            HttpResponseMessage postResponse = await testClient.PostAsync("api/teams", stringContent);

            postResponse.EnsureSuccessStatusCode();

            var getResponse = await testClient.GetAsync("api/teams");
            getResponse.EnsureSuccessStatusCode();

            string raw = await getResponse.Content.ReadAsStringAsync();

            List<Team> teams = JsonConvert.DeserializeObject<List<Team>>(raw);

            Assert.Single(teams);
            Assert.Equal("Zombie Team", teams[0].Name);
            Assert.Equal(teamZombie.ID, teams[0].ID);

        }
    }
}

using Microservice.Service.Controllers;
using Microservice.Service.Models;
using Microservice.Service.Repsitory;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Microservice.Test.UnitTests
{
    public class TeamsControllerTest
    {
        static MemoryTeamRepository repo= new MemoryTeamRepository();
        TeamsController controller = new TeamsController(repo);

        [Fact]
        public async void QueryTeamsList()
        {
            await controller.CreateTeam(new Team("Martin"));
            await controller.CreateTeam(new Team("Ruth"));
            var result = await controller.GetAllTeams() as OkObjectResult;

            var teams = new List<Team>(result.Value as List<Team>);

            Assert.Equal(2, teams.Count);
        }

        [Fact]
        public async void CreateTeamAddsTeamToList()
        {
            TeamsController controller = new TeamsController(repo);

            var teams =  await controller.GetAllTeams() as OkObjectResult;
            List<Team> original = new List<Team>(teams.Value as List<Team>);
            Team t = new Team("Sample");

            var result = await controller.CreateTeam(t);

            var newTeamsRaw = await controller.GetAllTeams() as OkObjectResult;

            List<Team> newTeams = new List<Team>(newTeamsRaw.Value as List<Team>);
            Assert.Equal(original.Count + 1, newTeams.Count);

            var sampleTeam = newTeams.FirstOrDefault(c => c.Name == "Sample");

            Assert.NotNull(sampleTeam);
        }
    }
}

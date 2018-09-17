using Microservice.Service.Controllers;
using Microservice.Service.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Microservice.Test
{
    public class TeamsConrollerTest
    {

        TeamsController controller = new TeamsController();

        [Fact]
        public void Test1()
        {
            List<Team> teams = new List<Team>(controller.GetAllTeams());

            Assert.Equal(2, teams.Count);
        }
    }
}

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

        //[Fact]
        //public async void QueryTeamsList()
        //{
        //    await controller.CreateTeam(new Team("Team"));
        //    await controller.CreateTeam(new Team("Team2"));
        //    var result = await controller.GetAllTeams() as OkObjectResult;

        //    var teams = new List<Team>(result.Value as List<Team>);

        //    Assert.Equal(2, teams.Count);
        //}

        [Fact]
        public async void CreateTeamAddsTeamToList()
        {
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

        [Fact]
        public async void GetAllTeamMembersFromTeam()
        {
            await controller.CreateTeam(new Team("Team3",Guid.NewGuid()));
            var okResult = await controller.GetAllTeams() as OkObjectResult;

            List<Team> teams = new List<Team>(okResult.Value as List<Team>);

            var softhouseTeam = teams.FirstOrDefault(t => t.Name == "Team3");
            int originalCount = softhouseTeam.Members.Count();
            Member member = new Member("Team", "Member1", Guid.NewGuid());
            Member member2 = new Member("Team", "Member2", Guid.NewGuid());
            Member member3 = new Member("Team", "Member3", Guid.NewGuid());
            await controller.AddNewTeamMember(softhouseTeam.ID, member);
            await controller.AddNewTeamMember(softhouseTeam.ID, member2);
            await controller.AddNewTeamMember(softhouseTeam.ID, member3);

            var membersResult = await controller.GetAllMembersFromTeam(softhouseTeam.ID) as OkObjectResult;
            var members = new List<Member>(membersResult.Value as List<Member>);

            Assert.Equal(originalCount + 3, members.Count);
        }

        [Fact]
        public async void GetSpecificTeamMember()
        {
            await controller.CreateTeam(new Team("Team3", Guid.NewGuid()));
            var okResult = await controller.GetAllTeams() as OkObjectResult;

            List<Team> teams = new List<Team>(okResult.Value as List<Team>);

            var softhouseTeam = teams.FirstOrDefault(t => t.Name == "Team3");
            int originalCount = softhouseTeam.Members.Count();
            Member member = new Member("Team", "Member1", Guid.NewGuid());
            Member member2 = new Member("Team", "Member2", Guid.NewGuid());
            Member member3 = new Member("Team", "Member3", Guid.NewGuid());
            await controller.AddNewTeamMember(softhouseTeam.ID, member);
            await controller.AddNewTeamMember(softhouseTeam.ID, member2);
            await controller.AddNewTeamMember(softhouseTeam.ID, member3);

            var result = await controller.GetTeamMember(softhouseTeam.ID, member.ID) as OkObjectResult;
            var selectedMember = result.Value as Member;

            Assert.Equal(member.ID, selectedMember.ID);
            Assert.Equal(member.FirstName, selectedMember.FirstName);
            Assert.Equal(member.LastName, selectedMember.LastName);
        }

        [Fact]
        public async void DeleteSpecificTeamMember()
        {
            await controller.CreateTeam(new Team("Team4", Guid.NewGuid()));
            var okResult = await controller.GetAllTeams() as OkObjectResult;

            List<Team> teams = new List<Team>(okResult.Value as List<Team>);

            var team = teams.FirstOrDefault(t => t.Name == "Team4");
            Member member = new Member("Team", "Member1", Guid.NewGuid());
            Member member2 = new Member("Team", "Member2", Guid.NewGuid());
            Member member3 = new Member("Team", "Member3", Guid.NewGuid());
            await controller.AddNewTeamMember(team.ID, member);
            await controller.AddNewTeamMember(team.ID, member2);
            await controller.AddNewTeamMember(team.ID, member3);

            int originalCount = team.Members.Count();

            await controller.DeleteTeamMember(team.ID, member3.ID);

            var result = await controller.GetAllMembersFromTeam(team.ID) as OkObjectResult;

            var currentTeamMemberCount = new List<Member>(result.Value as List<Member>);

            Assert.Equal(originalCount - 1, currentTeamMemberCount.Count());


        }
    }
}

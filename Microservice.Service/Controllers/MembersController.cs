using Microservice.Service.Interfaces;
using Microservice.Service.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Service.Controllers
{
    public class MembersController : Controller
    {

        private ITeamRepository _repo;
        private readonly IHttpLocationClient locationClient;

        public MembersController(ITeamRepository repo, IHttpLocationClient client)
        {
            _repo = repo;
            locationClient = client;
        }

        [HttpGet]
        [Route("api/teams/{id}/members")]
        public async Task<IActionResult> GetAllMembersFromTeam(Guid id)
        {
            return Ok(_repo.GetAllMembersFromTeam(id));
        }

        [HttpGet]
        [Route("api/teams/{id}/members/{memberId}")]
        public async Task<IActionResult> GetTeamMember(Guid id, Guid memberId)
        {
            var member = await _repo.GetTeamMemberAsync(id, memberId);
            var memberWithLocation = await GetLocationsForMembers(member);
            member.Location = memberWithLocation.Location;
            return Ok(member);
        }

        [HttpPost]
        [Route("api/teams/{id}/members")]
        public async Task<IActionResult> AddNewTeamMember(Guid id, [FromBody]Member newMember)
        {
            _repo.AddTeamMember(id, newMember);
            return Ok();
        }

        [HttpDelete]
        [Route("api/teams/{id}/members/{memberId}")]
        public async Task<IActionResult> DeleteTeamMember(Guid id, Guid memberId)
        {
            _repo.DeleteMember(id, memberId);
            return Ok();
        }

        private List<Member> GetLocationsForMembers(List<Member> members)
        {
            Parallel.ForEach(members, async (member) =>
            {
                member.Location = await locationClient.GetLatestForMemberAsync(member.ID);
            });

            return members;

        }

        private async Task<Member> GetLocationsForMembers(Member member)
        {
            member.Location = await locationClient.GetLatestForMemberAsync(member.ID);

            return member;
        }
    }
}

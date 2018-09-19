using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Service.Interfaces;
using Microservice.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Service.Controllers
{
    public class TeamsController : Controller
    {
        private ITeamRepository _repo;

        public TeamsController(ITeamRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Route("api/teams")]
        public async Task<IActionResult> GetAllTeams()
        {
            return Ok(_repo.GetTeams());
        }

        [HttpGet]
        [Route("api/teams/{id}")]
        public async Task<IActionResult> GetTeam(Guid id)
        {
            return Ok(_repo.GetTeam(id));
        }

        [HttpPost]
        [Route("api/teams")]
        public async Task<IActionResult> CreateTeam([FromBody]Team newTeam)
        {
            _repo.AddTeam(newTeam);
            return Ok(StatusCode(201));
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
            return Ok(_repo.GetTeamMember(id, memberId));
        }

        [HttpPost]
        [Route("api/teams/{id}/members")]
        public async Task<IActionResult> AddNewTeamMember(Guid id, [FromBody]Member newMember)
        {
            _repo.AddTeamMember(id, newMember);
            return Ok();
        }

        [HttpDelete]
        [Route("api/teams/{id}")]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            _repo.DeleteTeam(id);
            return Ok();
        }

        [HttpDelete]
        [Route("api/teams/{id}/members/{memberId}")]
        public async Task<IActionResult> DeleteTeamMember(Guid id,Guid memberId)
        {
            _repo.DeleteMember(id, memberId);
            return Ok();
        }
    }
}
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

        [HttpPost]
        [Route("api/teams")]
        public async Task<IActionResult> CreateTeam([FromBody]Team newTeam)
        {
            _repo.AddTeam(newTeam);
            return Ok(StatusCode(201));
        }
    }
}
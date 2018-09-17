using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Service.Controllers
{
    public class TeamsController : Controller
    {
        [HttpGet]
        public IEnumerable<Team> GetAllTeams()
        {
            var teamList = new List<Team>{new Team("Martin"),new Team("Ruth")};
            return teamList;
        }
    }
}
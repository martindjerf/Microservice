using Microservice.Service.Interfaces;
using Microservice.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Service.Repsitory
{
    public class MemoryTeamRepository : ITeamRepository
    {
        protected static ICollection<Team> _teams;

        public MemoryTeamRepository()
        {
            if(_teams == null)
            {
                _teams = new List<Team>();
            }
        }

        public MemoryTeamRepository(ICollection<Team> teams)
        {
            _teams = teams;
        }

        public void AddTeam(Team team)
        {
            _teams.Add(team);
        }

        public IEnumerable<Team> GetTeams()
        {
            return _teams;
        }
    }
}

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
            //_client = client;
        }

        public void AddTeam(Team team)
        {
            _teams.Add(team);
        }

        public IEnumerable<Team> GetTeams()
        {
            return _teams;
        }

        public IEnumerable<Member> GetAllMembersFromTeam(Guid id)
        {
            var team = _teams.Where(t => t.ID == id).FirstOrDefault();
            return team.Members;
        }

        public void AddTeamMember(Guid id, Member newMeber)
        {
            _teams.Where(t => t.ID == id).FirstOrDefault().Members.Add(newMeber);
        }

        public async Task<Member> GetTeamMemberAsync(Guid id, Guid memberId)
        {
            var team = _teams.Where(t => t.ID == id).FirstOrDefault();
            var member = team.Members.Where(m => m.ID == memberId).FirstOrDefault();            
            return member;
        }

        public async void DeleteMember(Guid id, Guid memberId)
        {
            var team = _teams.Where(t => t.ID == id).FirstOrDefault();
            var member = await GetTeamMemberAsync(id, memberId);
            team.Members.Remove(member);
        }

        public void DeleteTeam(Guid id)
        {
            var team = _teams.Where(t => t.ID == id).FirstOrDefault();
            _teams.Remove(team);
        }

        public Team GetTeam(Guid id)
        {
            var team =_teams.Where(t => t.ID == id).FirstOrDefault();           
            return team;

        }
    }
}

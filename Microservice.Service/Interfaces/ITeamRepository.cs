using Microservice.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Service.Interfaces
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams();
        void AddTeam(Team team);
        IEnumerable<Member> GetAllMembersFromTeam(Guid id);
        void AddTeamMember(Guid id, Member newMeber);
        Task<Member> GetTeamMemberAsync(Guid id, Guid memberId);
        void DeleteMember(Guid id, Guid memberId);
        void DeleteTeam(Guid id);
        Team GetTeam(Guid id);
    }
}

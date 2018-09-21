using Microservice.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Service.Interfaces
{
    public interface IHttpLocationClient
    {
        Task<LocationRecord> GetLatestForMemberAsync(Guid memberId);
    }
}

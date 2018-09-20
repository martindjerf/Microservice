using Microservice.LocationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.LocationService.Interfaces
{
    public interface ILocationRecordRepository
    {
        LocationRecord Add(LocationRecord locationRecord);
        LocationRecord Update(LocationRecord locationRecord);
        LocationRecord Get(Guid memberId, Guid recordId);
        LocationRecord Delete(Guid memberId, Guid recordId);
        LocationRecord GetLatestForMember(Guid memberId);
        ICollection<LocationRecord> AllForMember(Guid memberId);
        LocationRecord GetSpecificLocationRecordForMember(Guid memberId, Guid recordID);
    }
}

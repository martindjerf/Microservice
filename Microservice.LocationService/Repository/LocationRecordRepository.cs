using Microservice.LocationService.Interfaces;
using Microservice.LocationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.LocationService.Repository
{
    public class LocationRecordRepository : ILocationRecordRepository
    {
        protected static List<LocationRecord> _locationRecords;

        public LocationRecordRepository()
        {
            if(_locationRecords == null)
            {
                _locationRecords = new List<LocationRecord>();
            }
        }

        public LocationRecord Add(LocationRecord locationRecord)
        {
            _locationRecords.Add(locationRecord);
            return locationRecord;
        }

        public ICollection<LocationRecord> AllForMember(Guid memberId)
        {
            List<LocationRecord> memberLocationRecords = _locationRecords.Where(m => m.MemberID == memberId).ToList();
            return memberLocationRecords;
        }

        public LocationRecord Delete(Guid memberId, Guid recordId)
        {
            var locationRecordForMember = _locationRecords.Where(m => m.MemberID == memberId && m.ID == recordId).FirstOrDefault();
            _locationRecords.Remove(locationRecordForMember);
            return locationRecordForMember;
        }

        public LocationRecord Get(Guid memberId, Guid recordId)
        {
            return _locationRecords.Where(m => m.MemberID == memberId && m.ID == recordId).FirstOrDefault();
        }

        public LocationRecord GetLatestForMember(Guid memberId)
        {
            var records = _locationRecords.Where(m => m.MemberID == memberId).ToList();
            return records.OrderByDescending(o => o.Timestamp).FirstOrDefault();
        }

        public LocationRecord GetSpecificLocationRecordForMember(Guid memberId, Guid recordID)
        {
            return _locationRecords.Where(r => r.MemberID == memberId && r.ID == recordID).FirstOrDefault();
        }

        public LocationRecord Update(LocationRecord locationRecord)
        {
            var recordToUpdate = _locationRecords.Where(l => l.MemberID == locationRecord.MemberID).FirstOrDefault();
            _locationRecords.Remove(recordToUpdate);
            _locationRecords.Add(locationRecord);

            return locationRecord;

        }
    }
}

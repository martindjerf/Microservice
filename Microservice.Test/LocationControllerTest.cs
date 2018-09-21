using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microservice.LocationService;
using Microservice.LocationService.Controllers;
using Microservice.LocationService.Interfaces;
using Microservice.LocationService.Models;
using Microservice.LocationService.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Microservice.Test
{
    public class LocationControllerTest
    {
        private static ILocationRecordRepository repo = new LocationRecordRepository();
        private LocationController controller = new LocationController(repo);


        [Fact]
        public void AddLocationRecordAndFetchIt()
        {
            var locationRecord = new LocationRecord()
            {
                ID = Guid.NewGuid(),
                Altitude = 1200,
                Latitude = 54.12f,
                Longitude = 12.31f,
                MemberID = Guid.NewGuid(),
                Timestamp = GetUnixTimeStamp()
            };

            controller.AddLocation(locationRecord.MemberID, locationRecord);

            var fetchedLocationRecord = controller.GetSpecificLocationForMember(locationRecord.MemberID, locationRecord.ID) as OkObjectResult;

            var fetchedResult = fetchedLocationRecord.Value as LocationRecord;

            Assert.Equal(locationRecord.ID, fetchedResult.ID);
            

        }

        [Fact]
        public void AddMultipleLocationRecordsAndFetchLatest()
        {
            var memberId = Guid.NewGuid();
            long timeStamp = 0;
            for (int i = 0; i <= 4; i++)
            {
                var locationRecord = new LocationRecord()
                {
                    ID = Guid.NewGuid(),
                    Altitude = 1200,
                    Latitude = 54.12f,
                    Longitude = 12.31f,
                    MemberID = memberId,
                    Timestamp = GetUnixTimeStamp()
                };

                controller.AddLocation(locationRecord.MemberID, locationRecord);
                Thread.Sleep(1000);
                if (i >= 4)
                {
                    timeStamp = locationRecord.Timestamp;
                }
            }

            var results = controller.GetLatestLocationsForMember(memberId) as OkObjectResult;

            var fetchedLocationRecord = results.Value as LocationRecord;

            Assert.Equal(timeStamp, fetchedLocationRecord.Timestamp);

        }

        private long GetUnixTimeStamp()
        {
            var dateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTimeKind.Local);
            var dateTimeOffset = new DateTimeOffset(dateTime);
            var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();

            return unixDateTime;
        }
    }
}

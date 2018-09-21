using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservice.LocationService.Interfaces;
using Microservice.LocationService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.LocationService.Controllers
{
    public class LocationController : Controller
    {
        private ILocationRecordRepository _locationRecordRepository;

        public LocationController(ILocationRecordRepository locationRecordRepository)
        {
            _locationRecordRepository = locationRecordRepository;
        }

        [HttpPost]
        [Route("api/locations/{memberId}")]
        public IActionResult AddLocation(Guid memberId, [FromBody] LocationRecord record)
        {
            _locationRecordRepository.Add(record);
            return Created($"api/locations/{memberId}/{record.ID}", record);
        }

        [HttpGet]
        [Route("api/locations/{memberId}/{recordId}")]
        public IActionResult GetSpecificLocationForMember(Guid memberId, Guid recordId)
        {
            return Ok(_locationRecordRepository.GetSpecificLocationRecordForMember(memberId, recordId));
        }

        [HttpGet]
        [Route("api/locations/{memberId}")]
        public IActionResult GetLocationsForMember(Guid memberId)
        {
            return Ok(_locationRecordRepository.AllForMember(memberId));
        }

        [HttpGet]
        [Route("api/locations/{memberId}/latest")]
        public IActionResult GetLatestLocationsForMember(Guid memberId)
        {
            return Ok(_locationRecordRepository.GetLatestForMember(memberId));
        }
    }
}
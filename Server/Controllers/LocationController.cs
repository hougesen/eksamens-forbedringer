using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Server.Controllers

{
    [ApiController]
    [Route("api/locations")]
    public class LocationController : ControllerBase
    {
        private readonly uclweb_gr1Context _context;

        public LocationController(uclweb_gr1Context context)
        {
            _context = context;
        }

        // GET: api/locations/
        [HttpGet("")]
        async public Task<IActionResult> GetLocations() 
        {
            var location = await _context.Locations.Include(
                (l) => l.City).ThenInclude(
                    (city) => city.Country).Select(
                        (l) => new LocationRequest() 
                        {
                            LocationId = l.LocationId,
                            LocationAddress = l.LocationAddress,
                            LocationPostalCode = l.LocationPostalCode,
                            CityId = l.CityId ?? null,
                            CityName = l.City.CityName ?? null,
                            CountryId = l.City.CountryId ?? null,
                            CountryName = l.City.Country.CountryName ?? null,
                        }).ToListAsync();

            if (location != null && location.Count() > 0)
            {
                return new JsonResult(location);
            }

            return NotFound();
        }  

       // GET: api/locations/1
        [HttpGet("{locationId}")]
        async public Task<IActionResult> GetLocation(int locationId) 
        {
            var location = await _context.Locations.Where((l) => l.LocationId == locationId).Include(
                (l) => l.City).ThenInclude(
                    (city) => city.Country).Select(
                        (l) => new LocationRequest() 
                        {
                            LocationId = l.LocationId,
                            LocationAddress = l.LocationAddress,
                            LocationPostalCode = l.LocationPostalCode,
                            CityId = l.CityId ?? null,
                            CityName = l.City.CityName ?? null,
                            CountryId = l.City.CountryId ?? null,
                            CountryName = l.City.Country.CountryName ?? null,
                        }).FirstOrDefaultAsync();

            if (location != null)
            {
                return new JsonResult(location);
            }

            return NotFound();
        }   
    }


}
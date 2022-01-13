
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

namespace Server.Controllers

{
    [ApiController]
    [Route("api/routes")]
    public class RouteController : ControllerBase
    {
        private readonly uclweb_gr1Context _context;

        public RouteController(uclweb_gr1Context context)
        {
            _context = context;
        }

        // GET: api/routes/
        [HttpGet("")]
        async public Task<IActionResult> GetRoutes()
        {
            List<Route> routes = await _context.Routes.AsNoTracking().ToListAsync();

            if (routes != null && routes.Count > 0)
            {
                return new JsonResult(routes);
            }

            return NotFound();
        }

        [HttpPost("")]
        async public Task<IActionResult> CreateRoute([FromBody] CreateRouteBody body)
        {
            Route route = new()
            {
                RouteDescription = body.RouteDescription,
                RouteStartDate = DateTime.Parse(body.RouteStartDate),
                RouteEndDate = DateTime.Parse(body.RouteEndDate),
                RouteStartLocationId = body.RouteStartLocationId,
                RouteEndLocationId = body.RouteEndLocationId,
                RouteHighPriority = body.RouteHighPriority || false,
                RouteStatusId = 1,
                UserId = body.UserId,
                DepartmentId = body.DepartmentId,
                RouteEstTime = body.RouteEstTime
            };

            _context.Routes.Add(route);
            await _context.SaveChangesAsync();

            return new JsonResult(route);
        }

        [HttpDelete("{routeId}")]
        async public Task<IActionResult> DeleteRoute(int routeId)
        {
            var route = await _context.Routes.FindAsync(routeId);

            if (route == null)
            {
                return NotFound();
            }

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{routeId}")]
        async public Task<IActionResult> GetRoute(int routeId)
        {
            Route route = await _context.Routes.Where((r) => r.RouteId == routeId).AsNoTracking().FirstAsync();

            if (route == null)
            {
                return NotFound();
            }

            List<SignUpDriver> signups = await _context.SignUpDrivers.Where((s) => s.RouteId == routeId).AsNoTracking().ToListAsync();

            GetRouteResponse response = new()
            {
                route = route,
                signups = signups != null ? signups : null
            };

            return new JsonResult(response);
        }

        [HttpGet("user/{userId}")]
        async public Task<IActionResult> GetUserRoutes(int userId) {
            Route[] routes = await _context.Routes.Where((r) => r.userId == userId).AsNoTracking().ToListAsync();

            if (routes == null || routes.Length() == 0) {
                return NoContent();
            }

            return new JsonResult(routes);
        }
    }

    public class CreateRouteBody
    {
        public string RouteDescription { get; set; }
        public string RouteStartDate { get; set; }
        public string RouteEndDate { get; set; }
        public int RouteStartLocationId { get; set; }
        public int RouteEndLocationId { get; set; }
        public bool RouteHighPriority { get; set; }
        public int? RouteStatusId { get; set; }
        public int? UserId { get; set; }
        public int DepartmentId { get; set; }
        public int RouteEstTime { get; set; }
    }

    public class GetRouteResponse
    {
        public Route route { get; set; }
        public List<SignUpDriver> signups { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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

        [HttpDelete("delete/{routeId}")]
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
}
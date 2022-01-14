using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<Route> routes = await _context.Routes.OrderBy((r) => r.RouteStartDate).AsNoTracking().ToListAsync();

            if (routes != null && routes.Count() > 0)
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

        // PUT: api/routes/1
        [HttpPut("{routeId}")]
        async public Task<IActionResult> EditRoute([FromBody] Route body)
        {
            Route route = await _context.Routes.FindAsync(body.RouteId);
            if (route != null)
            {
                route.RouteDescription = body.RouteDescription;
                route.RouteStartDate = body.RouteStartDate;
                route.RouteEndDate = body.RouteEndDate;
                route.RouteStartLocationId = body.RouteStartLocationId;
                route.RouteEndLocationId = body.RouteEndLocationId;
                route.RouteHighPriority = body.RouteHighPriority;
                route.DepartmentId = body.DepartmentId;
                route.RouteEstTime = body.RouteEstTime;

                _context.Entry(route).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return new JsonResult(route);
            }
            return NotFound();
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
        async public Task<IActionResult> GetUserRoutes(int userId)
        {
            var routes = await _context.Routes.Where((r) => r.UserId == userId).OrderBy((r) => r.RouteStartDate).AsNoTracking().ToListAsync();

            if (routes == null || routes.Count() == 0)
            {
                return NoContent();
            }

            return new JsonResult(routes);
        }

        [HttpGet("available/{date}")]
        async public Task<IActionResult> GetAvailableRoutesByDate(string date)
        {
            // 03 12 1999 00 00 00 
            DateTime beginDayDate = DateTime.Parse(date);
            // 03 12 1999 23 59 59 
            DateTime endDayDate = beginDayDate.AddDays(1).AddSeconds(-1);

            var routes = await _context.Routes.Where((r) => r.UserId == null && r.RouteStartDate <= endDayDate && r.RouteStartDate >= beginDayDate).OrderBy((r) => r.RouteStartDate).AsNoTracking().ToListAsync();

            if (routes != null && routes.Count() > 0)
            {
                return new JsonResult(routes);
            }

            return NoContent();
        }

        [HttpPatch("assign/{routeId}/{userId}")]
        async public Task<IActionResult> AssignRouteUser(int routeId, int userId)
        {
            Route route = await _context.Routes.FindAsync(routeId);

            if (route != null)
            {
                route.UserId = userId;
                route.RouteStatusId = 2;

                _context.Entry(route).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    var driversAvailableDates = await _context.DriversAvailables.Where(
                        (d) => d.UserId == userId
                                && d.DriversAvailableDate <= route.RouteEndDate
                                && d.DriversAvailableDate >= route.RouteStartDate
                                ).ToListAsync();

                    if (driversAvailableDates != null && driversAvailableDates.Count() > 0)
                    {
                        _context.DriversAvailables.RemoveRange(driversAvailableDates);
                        await _context.SaveChangesAsync();
                    }


                    return new JsonResult(route);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }
            }

            return NotFound();
        }

        // PATCH: api/routes/completed/1
        [HttpPatch("completed/{routeId}")]
        public async Task<IActionResult> MarkRouteCompleted(int routeId)
        {
            Route route = await _context.Routes.FindAsync(routeId);

            if (route != null)
            {
                route.RouteStatusId = 3;
                _context.Entry(route).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return new JsonResult(route);
            }

            return NotFound();
        }
    }
}
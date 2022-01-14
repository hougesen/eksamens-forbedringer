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
    [Route("api/signups")]
    public class SignUpDriverController : ControllerBase
    {
        private readonly uclweb_gr1Context _context;

        public SignUpDriverController(uclweb_gr1Context context)
        {
            _context = context;
        }

        // POST: api/signups/1/1
        [HttpPost("{routeId}/{userId}")]
        async public Task<IActionResult> CreateNewSingUp(int routeId, int userId)
        {
            SignUpDriver signup = new()
            {
                RouteId = routeId,
                UserId = userId
            };

            _context.SignUpDrivers.Add(signup);
            await _context.SaveChangesAsync();

            return new JsonResult(signup);
        }

        // GET api/signups/route/1
        [HttpGet("route/{routeId}")]
        async public Task<IActionResult> GetRouteSignups(int routeId) 
        {
            var routeSignups = await _context.SignUpDrivers.Where(
                (s) => s.RouteId == routeId).Include(
                    (s) => s.User).Select(
                        (s)=> new User(){
                            UserId = s.User.UserId,
                            UserEmail = s.User.UserEmail,
                            UserFullName = s.User.UserFullName,
                            UserPhoneNumber = s.User.UserPhoneNumber,
                        }).AsNoTracking().ToListAsync();

            if (routeSignups != null && routeSignups.Count() > 0) 
            {
                return new JsonResult(routeSignups);
            }

            return NoContent();
        }

        // GET api/signups/user/1
        [HttpGet("user/{userId}")]
        async public Task<IActionResult> GetUserSignups(int userId) 
        {
            var signups = await _context.SignUpDrivers.Where((s) => s.UserId == userId).AsNoTracking().ToListAsync();

            if (signups != null && signups.Count() > 0) 
            {
                return new JsonResult(signups);
            }

            return NoContent();
        }
    }
}
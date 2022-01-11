using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Server.Controllers

{
    [ApiController]
    [Route("api/drivers")]
    public class DriverController : ControllerBase
    {
        private readonly uclweb_gr1Context _context;

        public DriverController(uclweb_gr1Context context)
        {
            _context = context;
        }

        // GET: api/drivers/
        [HttpGet("")]
        async public Task<IActionResult> GetDrivers()
        {
            List<User> drivers = await _context.Users.Where((u) => u.UserTypeId == 2).ToListAsync();

            if (drivers != null && drivers.Count > 0)
            {
                return new JsonResult(drivers);
            }

            return NotFound();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<DriverRequest> drivers = await _context.DriverInformations.Include(
                (d) => d.User).Select(
                    (d) => new DriverRequest()
                    {
                        UserId = d.UserId,
                        DriverInformationId = d.DriverInformationId,
                        UserEmail = d.User.UserEmail,
                        UserFullName = d.User.UserFullName,
                        UserPhoneNumber = d.User.UserPhoneNumber,
                        LocationId = d.LocationId ?? null,
                        DriverLicenceId = d.DriverLicenceId ?? null,
                        LorryLicenceId = d.LorryLicenceId ?? null,
                        Eucertificate = d.Eucertificate ?? null,
                    }).ToListAsync();

            if (drivers != null && drivers.Count() > 0)
            {
                return new JsonResult(drivers);
            }

            return NotFound();
        }

        // GET: api/drivers/available/1
        [HttpGet("available/{userId}")]
        async public Task<IActionResult> GetDriverAvailability(int userId)
        {

            List<DriversAvailable> dates = await _context.DriversAvailables.Where(
                (d) => d.UserId == userId).ToListAsync();

            if (dates != null && dates.Count() > 0)
            {
                return new JsonResult(dates);
            }

            return NoContent();
        }
    }
}
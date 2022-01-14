using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{

    [ApiController]
    [Route("api/login")]

    public class LoginController : ControllerBase
    {
        private readonly uclweb_gr1Context _context;

        public LoginController(uclweb_gr1Context context)
        {
            _context = context;
        }

        // POST: api/users/login
        [HttpPost()]
        async public Task<IActionResult> Login([FromBody] LoginRequest body)
        {
            User user = await _context.Users.Where((u) => u.UserEmail == body.email.ToLower() && u.UserPassword == body.password).FirstOrDefaultAsync();

            if (user != null)
            {

                return new JsonResult(user);
            }

            return NotFound();
        }
    }

    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}

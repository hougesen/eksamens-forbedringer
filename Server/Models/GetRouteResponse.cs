using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public class GetRouteResponse
    {
        public Route route { get; set; }

        public List<SignUpDriver> signups { get; set; }
    }
}

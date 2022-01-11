using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
    public partial class RouteStatus
    {
        public RouteStatus()
        {
            Routes = new HashSet<Route>();
        }

        public int RouteStatusId { get; set; }
        public string RouteStatusName { get; set; }

        public virtual ICollection<Route> Routes { get; set; }
    }
}

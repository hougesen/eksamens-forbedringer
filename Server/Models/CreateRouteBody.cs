using System.Collections.Generic;

#nullable disable

namespace Server.Models
{
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

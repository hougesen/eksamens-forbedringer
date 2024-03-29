﻿#nullable disable

namespace Server.Models
{
    public partial class SignUpDriver
    {
        public int UserId { get; set; }
        public int RouteId { get; set; }

        public virtual Route Route { get; set; }
        public virtual User User { get; set; }
    }
}

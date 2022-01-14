#nullable disable

namespace Server.Models
{
    public class DriverRequest
    {
        public int? UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserFullName { get; set; }

        public string UserPhoneNumber { get; set; }

        public int DriverInformationId { get; set; }

        public int? LocationId { get; set; }

        public int? DriverLicenceId { get; set; }

        public int? LorryLicenceId { get; set; }

        public int? Eucertificate { get; set; }
    }
}
#nullable disable

namespace Server.Models
{
    public class LocationRequest
    {
        public int LocationId { get; set; }

        public string LocationAddress { get; set; }

        public string LocationPostalCode { get; set; }

        public int? CityId { get; set; }

        public string? CityName { get; set; }

        public int? CountryId { get; set; }

        public string? CountryName { get; set; }
    }
}
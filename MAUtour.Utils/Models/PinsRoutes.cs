using Microsoft.EntityFrameworkCore;

namespace MAUtour.Utils.Models
{
    [PrimaryKey("Id")]
    public class PinsRoutes
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public UserRoutes UserRoutes { get; set; }
        public int PinId { get; set; }
        public UserPins UserPins { get; set; }
    }
}

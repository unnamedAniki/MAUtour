using Microsoft.EntityFrameworkCore;

namespace MAUtour.Utils.Models
{
    [PrimaryKey("Id")]
    public class UserPins
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int UserId { get; set; }
        public Users Users { get; set; }
        public int TypeId { get; set; }
        public TypeOfPins Type { get; set; }
        public virtual ICollection<PinsRoutes> Pins { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace MAUtour.Utils.Models
{
    [PrimaryKey("Id")]
    public class UserRoutes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public Users Users { get; set; }
        public int TypeId { get; set; }
        public TypeOfRoutes Type { get; set; }
        public virtual ICollection<PinsRoutes> Routes { get; set; } = new HashSet<PinsRoutes>();
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.Models
{
    [PrimaryKey("Id")]
    public class UserRoutes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public Users Users { get; set; }
        public int TypeId { get; set; }
        public TypeOfRoutes Type { get; set; }
        public virtual ICollection<PinsRoutes> Routes { get; set; } = new List<PinsRoutes>();
    }
}

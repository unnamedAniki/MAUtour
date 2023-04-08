using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.Models
{
    [PrimaryKey("Id")]
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AvailablePins { get; set; } = 5;
        public int AvailableRoutes { get; set; } = 2;
        public DateTime StartPaidDate { get; set; }
        public DateTime ExpiredPaidDate { get; set; }
        public virtual ICollection<UserPins> Pins { get; set; } = new List<UserPins>();
        public virtual ICollection<UserRoles> Roles { get; set; } = new List<UserRoles>(); 
        public virtual ICollection<UserRoutes> Routes { get; set; } = new List<UserRoutes>();
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.Models
{
    public class UserRoles
    {
        public int UserId { get; set; }
        public Users Users { get; set; }
        public int RoleId { get; set; }
        public Roles Roles { get; set; }
    }
}

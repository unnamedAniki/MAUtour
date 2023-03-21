using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Utils.Models
{
    public class PinsRoutes
    {
        public int RouteId { get; set; }
        public UserRoutes UserRoutes { get; set; }
        public int PinsId { get; set; }
        public UserPins UserPins { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Local.Models
{
    public class RouteTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Routes> Routes { get; set; }
    }
}

using MAUtour.Local.DBConnect;
using MAUtour.Local.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Local.Repository.Interfaces
{
    internal class RouteTypesRepository : Repository<RouteTypes>, IRouteTypesRepository
    {
        public RouteTypesRepository(LocalContext _context): base(_context) { }
    }
}

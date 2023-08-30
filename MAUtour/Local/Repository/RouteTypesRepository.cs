using MAUtour.Local.DBConnect;
using MAUtour.Local.Models;
using MAUtour.Local.Repository.Interfaces;

namespace MAUtour.Local.Repository
{
    internal class RouteTypesRepository : Repository<RouteTypes>, IRouteTypesRepository
    {
        public RouteTypesRepository(LocalContext _context): base(_context) { }
    }
}

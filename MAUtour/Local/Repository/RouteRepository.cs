using MAUtour.Local.DBConnect;
using MAUtour.Local.Models;
using MAUtour.Local.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace MAUtour.Local.Repository
{
    internal class RouteRepository : Repository<Routes>, IRoutesRepository
    {
        private readonly LocalContext _context;
        public RouteRepository(LocalContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<bool> AddRoutePinsAsync(IEnumerable<RoutePins> pins)
        {
            try
            {
                await _context.LocalRoutesPins.AddRangeAsync(pins);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetLastAddedRouteAsync()
        {
            var route = await _context.LocalRoutes.OrderBy(p=>p.Id).LastAsync();
            return route.Id;
        }

        public async Task<IEnumerable<RoutePins>> GetAllPinsOfRouteAsync(int route_id)
        {
            var route = await _context.LocalRoutesPins.Where(p=>p.RoutesId == route_id).ToListAsync();
            return route;
        }
    }
}

using MAUtour.Local.Models;

namespace MAUtour.Local.Repository.Interfaces
{
    public interface IRoutesRepository : IRepository<Routes>
    {
        Task<bool> AddRoutePinsAsync(IEnumerable<RoutePins> pins);
        Task<IEnumerable<RoutePins>> GetAllPinsOfRouteAsync(int route_id);
        Task<int> GetLastAddedRouteAsync();
    }
}

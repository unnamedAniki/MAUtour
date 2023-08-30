using MAUtour.Local.Repository.Interfaces;

namespace MAUtour.Local.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IPinRepository pinRepository { get; }
        IPinTypesRepository pinTypesRepository { get; }
        IRoutesRepository routesRepository { get; }
        IRouteTypesRepository routeTypesRepository { get; }
        Task<int> CommitAsync();    
    }
}

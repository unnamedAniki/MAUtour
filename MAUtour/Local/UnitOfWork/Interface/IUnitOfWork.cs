using MAUtour.Local.Repository.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

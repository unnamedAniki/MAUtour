using MAUtour.Local.DBConnect;
using MAUtour.Local.Repository;
using MAUtour.Local.Repository.Interfaces;
using MAUtour.Local.UnitOfWork.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUtour.Local.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly LocalContext _context;
        private PinRepository _pinRepository;
        private PinTypesRepository _pinTypesRepository;
        private RouteRepository _routeRepository;
        private RouteTypesRepository _routeTypesRepository;
        private bool _disposed = false;
        public IPinRepository pinRepository => _pinRepository ??= new PinRepository(_context);
        public IPinTypesRepository pinTypesRepository => _pinTypesRepository ??= new PinTypesRepository(_context);
        public IRoutesRepository routesRepository => _routeRepository ??= new RouteRepository(_context);
        public IRouteTypesRepository routeTypesRepository => _routeTypesRepository ??= new RouteTypesRepository(_context);
        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public UnitOfWork(LocalContext context) => this._context = context ?? throw new ArgumentNullException(nameof(context));
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if(disposing)
                {
                    _context?.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

using DeliveryTermsDL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsDL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SupplyChainContext _context;
        private ISaDeliveryTermRepository? _saDeliveryTerm;

        public UnitOfWork(SupplyChainContext context)
        {
            _context = context;
        }

        public ISaDeliveryTermRepository saDeliveryTerm
        {
            get
            {
                if (_saDeliveryTerm == null)
                {
                    _saDeliveryTerm = new SaDeliveryTermRepository(_context);
                }
                return _saDeliveryTerm;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}

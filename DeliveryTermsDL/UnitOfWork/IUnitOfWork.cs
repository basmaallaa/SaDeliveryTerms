using DeliveryTermsDL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsDL.UnitOfWork
{
    public interface IUnitOfWork
    {
        ISaDeliveryTermRepository saDeliveryTerm { get; }
        void Commit();
    }
}

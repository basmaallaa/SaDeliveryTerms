using DeliveryTermsDL.Models.SupplyChain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsDL.Repositories
{
    public interface ISaDeliveryTermRepository
    {
        List<SaDeliveryTerm> GetAll();
        SaDeliveryTerm? GetByCode(int code);
        void Add(SaDeliveryTerm model);
        void Edit(SaDeliveryTerm model);
        void Delete(SaDeliveryTerm model);
    }
}

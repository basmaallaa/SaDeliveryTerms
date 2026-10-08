using DeliveryTermsDL.Models.SupplyChain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTermsDL.Repositories
{
    public class SaDeliveryTermRepository : ISaDeliveryTermRepository
    {
        private readonly SupplyChainContext _context;
        public SaDeliveryTermRepository(SupplyChainContext context)
        {
            _context = context;
        }

        public List<SaDeliveryTerm> GetAll()
        {
            return _context.SaDeliveryTerms.AsNoTracking().ToList();
        }

        public SaDeliveryTerm? GetByCode(int code)
        {
            return _context.SaDeliveryTerms.FirstOrDefault(x => x.Code == code);
        }

        public void Add(SaDeliveryTerm model)
        {
            _context.SaDeliveryTerms.Add(model);
        }

        public void Edit(SaDeliveryTerm model)
        {
            _context.SaDeliveryTerms.Update(model);
        }
        public void Delete(SaDeliveryTerm model)
        {
            _context.SaDeliveryTerms.Remove(model);
        }
    }
}

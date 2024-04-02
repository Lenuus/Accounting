using Microsoft.EntityFrameworkCore;
using Accounting.Domain;
using Accounting.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application
{
    public class Repository<Table> : IRepository<Table> where Table : class, IBaseEntity
    {
        private readonly MainDbContext _context;
        private DbSet<Table> _table;

        public Repository(MainDbContext context)
        {
            _context = context;
            _table = _context.Set<Table>();
        }
        public async Task<Table> Create(Table entity)
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public async Task Delete(Table entity)
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var query = _table.Find(id);
            if (query == null)
                throw new Exception("Not Found");

            _table.Remove(query);
            await _context.SaveChangesAsync();
        }


        public IQueryable<Table> GetAll()
        {
            return _table;
        }

        public async Task<Table> GetById(Guid id)
        {
            var query = await _table.FindAsync(id);
            if (query == null)
                throw new Exception("Not Found");

            return query;
        }
        public IQueryable<CorporationRecord> GetByReferenceId(Guid referenceId)
        {
            var query = _context.CorporationRecords.Where(po => po.ReferenceId == referenceId);
            if (query == null)
                throw new Exception("Not Found");
            return query;
        }

        public async Task Update(Table entity)
        {
            _table.Update(entity);

            await _context.SaveChangesAsync();

        }
        public IQueryable<ProductOrder> GetProductOrdersByOrderId(Guid orderId)
        {
            var query = _context.ProductOrders.Where(po => po.OrderId == orderId);
            if (!query.Any())
                throw new Exception("No Product Orders found for the given Order ID");
            return query;
        }

        public Task<CorporationRecord> GetCorpRecordByReferenceId(Guid ReferenceId)
        {
            var query = _context.CorporationRecords.FirstOrDefaultAsync(po => po.ReferenceId == ReferenceId);
            if (query == null)
                throw new Exception("No Corp Record found for the given Ref ID");
            return query;
        }
    }
}

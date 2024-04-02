using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application
{
    public interface IRepository<Table> where Table : IBaseEntity
    {
        IQueryable<Table> GetAll();

        Task<Table> GetById(Guid id);

        IQueryable<CorporationRecord> GetByReferenceId(Guid referenceId);

        Task DeleteById(Guid id);

        Task Delete(Table entity);

        Task Update(Table entity);

        Task<Table> Create(Table entity);

        #region Custom
        IQueryable<ProductOrder> GetProductOrdersByOrderId(Guid orderId);

        Task<CorporationRecord> GetCorpRecordByReferenceId(Guid ReferenceId);

        #endregion

    }
}

using Accounting.Common.Helpers;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.Domain;

namespace Accounting.Persistence.Interceptors
{
    public class UpdateAuditInterceptor : SaveChangesInterceptor
    {
        private readonly IClaimManager _claimManager;

        public UpdateAuditInterceptor(IClaimManager claimManager)
        {
            _claimManager = claimManager;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Modified, Entity: ISoftUpdatable update }) continue;

                update.UpdatedDate = DateTime.UtcNow;
                update.UpdatedById = _claimManager.GetUserId();
            }

            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Modified, Entity: ISoftUpdatable update }) continue;

                update.UpdatedDate = DateTime.UtcNow;
                update.UpdatedById = _claimManager.GetUserId();
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}

using Accounting.Common.Helpers;
using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Persistence.Interceptors
{
    public class CreateInterceptor : SaveChangesInterceptor
    {
        private readonly IClaimManager _claimManager;

        public CreateInterceptor(IClaimManager claimManager)
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
                if (entry is not { State: EntityState.Added, Entity: ISoftCreatable create }) continue;

                create.InsertedDate = DateTime.UtcNow;
                create.InsertedById = _claimManager.GetUserId();
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
                if (entry is not { State: EntityState.Added, Entity: ISoftCreatable create }) continue;

                create.InsertedDate = DateTime.UtcNow;
                create.InsertedById = _claimManager.GetUserId();
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}



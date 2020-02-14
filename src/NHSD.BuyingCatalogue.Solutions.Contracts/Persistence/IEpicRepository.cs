﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface IEpicRepository
    {
        Task<int> GetMatchingEpicIdsAsync(IEnumerable<string> epicIds, CancellationToken cancellationToken);
    }
}

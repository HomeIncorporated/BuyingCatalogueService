﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.SolutionLists.Application.Domain;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;
using NHSD.BuyingCatalogue.SolutionLists.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.SolutionLists.Application.Persistence
{
    internal sealed class SolutionListReader
    {
        /// <summary>
        /// Data access layer for the <see cref="SolutionList"/> entity.
        /// </summary>
        private readonly ISolutionListRepository _solutionListRepository;

        public SolutionListReader(ISolutionListRepository solutionListRepository)
            => _solutionListRepository = solutionListRepository;

        public async Task<SolutionList> ListAsync(
            IEnumerable<ICapabilityReference> capabilityReferences,
            bool foundationOnly,
            string supplierId,
            CancellationToken cancellationToken)
        {
            if (capabilityReferences is null)
            {
                throw new ArgumentNullException(nameof(capabilityReferences));
            }

            return new SolutionList(capabilityReferences,
                await _solutionListRepository.ListAsync(foundationOnly, supplierId, cancellationToken));
        }
    }
}


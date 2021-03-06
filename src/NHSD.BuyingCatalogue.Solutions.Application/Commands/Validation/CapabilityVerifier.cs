﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateCapabilities;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class CapabilityVerifier : IVerifier<UpdateCapabilitiesCommand, ISimpleResult>
    {
        private readonly ISolutionCapabilityRepository _solutionCapabilityRepository;
        private readonly VerifyCapabilityResult _verifyCapabilityResult;

        public CapabilityVerifier(ISolutionCapabilityRepository solutionCapabilityRepository)
        {
            _solutionCapabilityRepository = solutionCapabilityRepository;
            _verifyCapabilityResult = new VerifyCapabilityResult();
        }

        public async Task<ISimpleResult> VerifyAsync(UpdateCapabilitiesCommand command)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var result = await CheckCapabilityReferenceExists(command.NewCapabilitiesReferences, new CancellationToken()).ConfigureAwait(false);
            if (!result)
            {
                _verifyCapabilityResult.ValidCapabilityList.Add("capabilities");
            }
            return _verifyCapabilityResult;
        }

        public async Task<bool> CheckCapabilityReferenceExists(IEnumerable<string> capabilitiesToMatch,
            CancellationToken cancellationToken)
        {
            var count = await _solutionCapabilityRepository.GetMatchingCapabilitiesCountAsync(capabilitiesToMatch,
                cancellationToken).ConfigureAwait(false);

            return count == capabilitiesToMatch.ToList().Count;
        }
    }
}

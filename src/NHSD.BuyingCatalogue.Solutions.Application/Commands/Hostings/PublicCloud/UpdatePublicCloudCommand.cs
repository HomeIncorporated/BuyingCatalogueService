﻿using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.Hostings;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Hostings.PublicCloud
{
    public sealed class UpdatePublicCloudCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }

        public IUpdatePublicCloudData Data { get; }

        public UpdatePublicCloudCommand(string solutionId, IUpdatePublicCloudData data)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}

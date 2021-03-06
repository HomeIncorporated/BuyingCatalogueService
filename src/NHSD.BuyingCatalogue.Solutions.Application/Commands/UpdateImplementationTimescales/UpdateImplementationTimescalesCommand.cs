﻿using System;
using MediatR;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales
{
    public sealed class UpdateImplementationTimescalesCommand : IRequest<ISimpleResult>
    {
        public string SolutionId { get; }
        public string Description { get; }

        public UpdateImplementationTimescalesCommand(string solutionId, string description)
        {
            SolutionId = solutionId ?? throw new ArgumentNullException(nameof(solutionId));
            Description = description;
        }
    }
}

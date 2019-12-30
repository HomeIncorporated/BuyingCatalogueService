using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.API.Controllers
{
    internal static class ContactsMaxLengthResultExtensions
    {
        internal static ActionResult ToActionResult(this ContactsMaxLengthResult validationResult) =>
            validationResult.IsValid
                ? (ActionResult)new NoContentResult()
                : new BadRequestObjectResult(validationResult.ToContactsValidationResult());

        private static Dictionary<string, Dictionary<string, string>> ToContactsValidationResult(this ContactsMaxLengthResult contactsMaxLengthResult) =>
            new Dictionary<string, Dictionary<string, string>>()
                .AddIfInvalid(contactsMaxLengthResult.Contact1Result, "contact-1")
                .AddIfInvalid(contactsMaxLengthResult.Contact2Result, "contact-2");

        private static Dictionary<string, Dictionary<string, string>> AddIfInvalid(this Dictionary<string, Dictionary<string, string>> result, MaxLengthResult maxLengthResult, string name)
        {
            if (!maxLengthResult.IsValid)
            {
                result.Add(name, maxLengthResult.ToDictionary());
            }

            return result;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Application.Domain.Suppliers;
using NHSD.BuyingCatalogue.Solutions.Contracts;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    /// <summary>
    /// A product and/or service provided by a supplier.
    /// </summary>
    internal sealed class Solution
    {
        /// <summary>
        /// Id of the solution.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the solution, as displayed to a user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Record of the latest date the solution was modified
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Full description of the solution, as displayed to the user.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Summary of the solution, as displayed to a user.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets a list of features.
        /// </summary>
        public IEnumerable<string> Features { get; set; }

        /// <summary>
        /// Gets or sets a road map description.
        /// </summary>
        public RoadMap RoadMap { get; set; }

        /// <summary>
        /// Gets or sets an integration.
        /// </summary>
        public Integrations Integrations { get; set; }

        /// <summary>
        /// A link to provide more information about a solution.
        /// </summary>
        public string AboutUrl { get; set; }

        /// <summary>
        /// Status of this instance in relation to the supplier.
        /// </summary>
        public SupplierStatus SupplierStatus { get; }

        /// <summary>
        /// Marketing information related to the clients application.
        /// </summary>
        public ClientApplication ClientApplication { get; set; }

        /// <summary>
        /// Is this a foundation solution?
        /// </summary>
        public bool IsFoundation { get; set; }

        /// <summary>
        /// Capabilities claimed by the solution
        /// </summary>
        public IEnumerable<ClaimedCapability> Capabilities { get; set; }

        /// <summary>
        /// The contacts for the solution
        /// </summary>
        public IEnumerable<Contact> Contacts { get; set; }

        /// <summary>
        /// The publishing status of the solution
        /// </summary>
        public PublishedStatus PublishedStatus { get; set; }

        /// <summary>
        /// The hosting of the solution
        /// </summary>
        public Hosting Hosting { get; set; }

        /// <summary>
        /// The supplier of the solution
        /// </summary>
        public SolutionSupplier Supplier { get; set; }

        /// <summary>
        /// Gets or sets an implementation timescales.
        /// </summary>
        public ImplementationTimescales ImplementationTimescales { get; set; }

        /// <summary>
        /// Gets or sets the solution document for the solution.
        /// </summary>
        public SolutionDocument SolutionDocument { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solution" /> class.
        /// </summary>
        internal Solution(
            ISolutionResult solutionResult,
            IEnumerable<ISolutionCapabilityListResult> solutionCapabilityListResult,
            IEnumerable<IMarketingContactResult> contactResult,
            ISolutionSupplierResult solutionSupplierResult,
            IDocumentResult documentResult,
            IEnumerable<ISolutionEpicListResult> solutionEpicListResults)
        {
            var contactResultList = contactResult.ToList();
            var solutionEpicsByCapability = solutionEpicListResults?.ToLookup(e => e.CapabilityId);
            Id = solutionResult.Id;
            Name = solutionResult.Name;
            LastUpdated = GetLatestLastUpdated(solutionResult, contactResultList);
            Summary = solutionResult.Summary;
            Description = solutionResult.Description;
            Features = string.IsNullOrWhiteSpace(solutionResult.Features)
                ? new List<string>()
                : JsonConvert.DeserializeObject<IEnumerable<string>>(solutionResult.Features);
            Integrations = new Integrations
            {
                Url = solutionResult.IntegrationsUrl, DocumentName = documentResult?.IntegrationDocumentName
            };
            ImplementationTimescales =
                new ImplementationTimescales {Description = solutionResult.ImplementationTimescales};
            AboutUrl = solutionResult.AboutUrl;
            RoadMap = new RoadMap
            {
                Summary = solutionResult.RoadMap, DocumentName = documentResult?.RoadMapDocumentName
            };
            ClientApplication = string.IsNullOrWhiteSpace(solutionResult.ClientApplication)
                ? new ClientApplication()
                : JsonConvert.DeserializeObject<ClientApplication>(solutionResult.ClientApplication);
            IsFoundation = solutionResult.IsFoundation;
            Capabilities = solutionCapabilityListResult.Select(c =>
                new ClaimedCapability(c, solutionEpicsByCapability?[c.CapabilityId]));
            Contacts = contactResultList.Select(c => new Contact(c));
            PublishedStatus = solutionResult.PublishedStatus;

            Hosting = string.IsNullOrWhiteSpace(solutionResult.Hosting)
                ? new Hosting()
                : JsonConvert.DeserializeObject<Hosting>(solutionResult.Hosting);
            Supplier = solutionSupplierResult != null ? new SolutionSupplier(solutionSupplierResult) : new SolutionSupplier();

            SolutionDocument = new SolutionDocument(documentResult?.SolutionDocumentName);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Solution" /> class.
        /// </summary>
        public Solution()
        {
            SupplierStatus = SupplierStatus.Draft;
            PublishedStatus = PublishedStatus.Draft;
        }

        private static DateTime GetLatestLastUpdated(ISolutionResult solutionResult,
            IList<IMarketingContactResult> contactResult) =>
            new List<DateTime>
            {
                solutionResult.LastUpdated,
                solutionResult.SolutionDetailLastUpdated,
                contactResult?.Any() == true ? contactResult.Max(x => x.LastUpdated) : DateTime.MinValue
            }.Max();
    }
}

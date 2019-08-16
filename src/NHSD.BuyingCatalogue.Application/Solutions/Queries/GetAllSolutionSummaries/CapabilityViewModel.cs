﻿namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAll
{
	/// <summary>
	/// Provides the view representation for the <see cref="Capability"/> entity.
	/// </summary>
	public sealed class CapabilityViewModel
	{
		/// <summary>
		/// Identifier of the capability.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Name of the capability.
		/// </summary>
		public string Name { get; set; }
	}
}
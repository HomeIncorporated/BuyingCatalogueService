﻿using NHSD.BuyingCatalogue.API.Infrastructure.Filters;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace NHSD.BuyingCatalogue.API.Extensions
{
	/// <summary>
	/// Extends the functionality for the <see cref="IServiceCollection"/> class.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the project based database factory for the persistence layer.
		/// </summary>
		/// <param name="services">The collection of service descriptors.</param>
		/// <returns>The extended service collection instance.</returns>
		public static IServiceCollection AddCustomDbFactory(this IServiceCollection services)
		{
			services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

			return services;
		}

		/// <summary>
		/// Adds any application repositories.
		/// </summary>
		/// <param name="services">The collection of service descriptors.</param>
		/// <returns>The extended service collection instance.</returns>
		public static IServiceCollection AddCustomRepositories(this IServiceCollection services)
		{
			services.AddSingleton<ISolutionRepository, SolutionRepository>();

			return services;
		}

		/// <summary>
		/// Adds the custom swagger settings for application.
		/// </summary>
		/// <param name="services">The collection of service descriptors.</param>
		/// <param name="configuration"></param>
		/// <returns>The extended service collection instance.</returns>
		public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSwaggerGen(options =>
			{
				options.DescribeAllEnumsAsStrings();
				options.SwaggerDoc("v1", new Info
				{
					Title = "Solutions API",
					Version = "v1",
					Description = "NHS Digital GP IT Buying Catalogue HTTP API",
				});
			});

			return services;
		}

		/// <summary>
		/// Adds the MVC controllers and custom settings.
		/// </summary>
		/// <param name="services">The collection of service descriptors.</param>
		/// <returns>The extended service collection instance.</returns>
		public static IServiceCollection AddCustomMvc(this IServiceCollection services)
		{
			services.AddMvc(options => 
			{
				options.Filters.Add(typeof(CustomExceptionFilter));
			})
			.AddJsonOptions((jsonOptions) => 
			{
				jsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			})
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
			.AddControllersAsServices();

			return services;
		}
	}
}
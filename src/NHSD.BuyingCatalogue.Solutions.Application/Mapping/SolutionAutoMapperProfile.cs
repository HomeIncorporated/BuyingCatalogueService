using AutoMapper;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary;
using NHSD.BuyingCatalogue.Solutions.Application.Domain;
using NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Mapping
{
    public sealed class SolutionAutoMapperProfile : Profile
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SolutionAutoMapperProfile"/> class.
        /// </summary>
        public SolutionAutoMapperProfile()
        {
            CreateMap<UpdateSolutionSummaryViewModel, Solution>()
                .ForMember(destination => destination.AboutUrl, options => options.MapFrom(source => source.Link));
            CreateMap<UpdateSolutionFeaturesViewModel, Solution>()
                .ForMember(destination => destination.Features, options => options.MapFrom(source => source.Listing));

            CreateMap<Solution, SolutionDto>();
            CreateMap<Solution, ISolution>().As<SolutionDto>();
            CreateMap<ClientApplication, ClientApplicationDto>();
            CreateMap<ClientApplication, IClientApplication>().As<ClientApplicationDto>();
            CreateMap<Plugins, PluginsDto>();
            CreateMap<Plugins, IPlugins>().As<PluginsDto>();
            CreateMap<Contact, ContactDto>();
            CreateMap<Contact, IContact>().As<ContactDto>();
            CreateMap<MobileOperatingSystems, MobileOperatingSystemsDto>();
            CreateMap<MobileOperatingSystems, IMobileOperatingSystems>().As<MobileOperatingSystemsDto>();
            CreateMap<MobileConnectionDetails, MobileConnectionDetailsDto>();
            CreateMap<MobileConnectionDetails, IMobileConnectionDetails>().As<MobileConnectionDetailsDto>();
            CreateMap<MobileMemoryAndStorage, MobileMemoryAndStorageDto>();
            CreateMap<MobileMemoryAndStorage, IMobileMemoryAndStorage>().As<MobileMemoryAndStorageDto>();
            CreateMap<MobileThirdParty, MobileThirdPartyDto>();
            CreateMap<MobileThirdParty, IMobileThirdParty>().As<MobileThirdPartyDto>();
        }
    }
}

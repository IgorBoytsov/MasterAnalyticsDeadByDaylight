using AutoMapper;
using DBDAnalytics.CatalogService.Domain.ValueObjects.KillerPerkCategory;

namespace DBDAnalytics.CatalogService.Application.AutoMappers.ValueObjectProfiles
{
    public sealed class KillerPerkCategoryIdProfile : Profile
    {
        public KillerPerkCategoryIdProfile() => CreateMap<KillerPerkCategoryId, int>().ConvertUsing(src => src.Value);
    }
}
using AutoMapper;
using BaseApp.Domain.Common;

namespace BaseApp.Application.Common.Mappings.BaseMapping
{
    public abstract class BaseCreateProfile<TSource, TDestination>
        : Profile
        where TDestination : BaseEntity
    {
        protected BaseCreateProfile()
        {
            CreateMap<TSource, TDestination>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.RowVersion, opt => opt.Ignore());
        }
    }
}

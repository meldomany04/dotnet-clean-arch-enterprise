using AutoMapper;
using BaseApp.Domain.Common;

namespace BaseApp.Application.Common.Mappings.BaseMapping
{
    public abstract class BaseModifyProfile<TSource, TDestination>
        : Profile
        where TDestination : BaseEntity
    {
        protected BaseModifyProfile()
        {
            CreateMap<TSource, TDestination>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedAt, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.RowVersion, opt => opt.Ignore());
        }
    }
}
using AutoMapper;
using Mmdesign.Models;
using Mmdesign.Models.Entity;

namespace Mmdesign
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<MenuViewModel, Menu>();
            Mapper.CreateMap<CategoryViewModel, Category>();
            Mapper.CreateMap<ProjectModel, Project>();
        }
    }
}
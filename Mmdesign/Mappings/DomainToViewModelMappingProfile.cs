using AutoMapper;
using Mmdesign.Models;
using Mmdesign.Models.Entity;

namespace Mmdesign
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Menu, MenuViewModel>();
            Mapper.CreateMap<Category, CategoryViewModel>();
            Mapper.CreateMap<Project, ProjectModel>();
            Mapper.CreateMap<ProjectImage, ProjectImageModel>();
        }
    }
}
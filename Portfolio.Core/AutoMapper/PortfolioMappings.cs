using AutoMapper;
using Portfolio.Domain.Dtos;
using Portfolio.Domain.Dtos.Projects;
using Portfolio.Domain.Dtos.SkillGroup;
using Portfolio.Domain.Models;

namespace Portfolio.Core.AutoMapper
{
    public class PortfolioMappings : Profile
    {
        public PortfolioMappings()
        {
            CreateMap<AboutMe, AboutMeDto>();
            CreateMap<AboutMeDto, AboutMe>().ForMember(x => x.Id, options => options.Ignore());

            CreateMap<Message, MessageDto>();
            CreateMap<MessageDto, Message>();

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();

            CreateMap<CreateUpdateProject, ProjectDto>();

            CreateMap<CreateUpdateSkill, Skill>();
            CreateMap<Skill, SkillDto>();
            CreateMap<SkillDto, Skill>();

            CreateMap<CreateUpdateSkillGroupDto, SkillGroup>();
            CreateMap<SkillGroup, SkillGroupDto>();
            CreateMap<SkillGroupDto, SkillGroup>();
        }
    }
}

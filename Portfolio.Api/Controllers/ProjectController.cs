using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Core.Interfaces;
using Portfolio.Domain.Dtos.Projects;
using Portfolio.Domain.Models;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        #region Fields

        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _projectService;
        private readonly ISkillService _skillService;
        private readonly IMapper _mapper; 

        #endregion

        #region Constructor

        public ProjectController(ILogger<ProjectController> logger, IProjectService projectService, IMapper mapper, ISkillService skillService)
        {
            _logger = logger;
            _projectService = projectService;
            _mapper = mapper;
            _skillService = skillService;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IEnumerable<ProjectDto>> Get()
        {
            var projects = await _projectService.Get();

            foreach (var skill in projects.SelectMany(x => x.Skills))
            {
                skill.Projects = null;
            }

            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        [HttpPost]
        public async Task<ProjectDto> Create(CreateUpdateProject model)
        {
            var project = _mapper.Map<Project>(model);

            await _projectService.Create(project);
            return _mapper.Map<ProjectDto>(project);
        }

        [HttpPut]
        public async Task<ProjectDto> Update(CreateUpdateProject model)
        {
            var project = _mapper.Map<Project>(model);
            project = await _projectService.Update(project);

            foreach (var skill in project.Skills)
            {
                skill.Projects = null;
            }

            return _mapper.Map<ProjectDto>(project);
        }

        [HttpPut("UpdateSkills")]
        public async Task<ProjectDto> UpdateSkills(UpdateProjectSkills model)
        {
            var skills = await _skillService.GetSkillsByIds(model.SkillIds);
            var project = await _projectService.UpdateSkills(model.ProjectId, skills);

            foreach (var skill in project.Skills)
            {
                skill.Projects = null;
            }

            return _mapper.Map<ProjectDto>(project);
        }

        [HttpDelete]
        public Task Delete(int id)
        {
            return _projectService.Delete(id);
        }

        #endregion
    }
}

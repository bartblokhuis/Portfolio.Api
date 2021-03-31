using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Core.Interfaces;
using Portfolio.Domain.Dtos;
using Portfolio.Domain.Dtos.SkillGroup;
using Portfolio.Domain.Models;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillGroupController : ControllerBase
    {
        #region Fields

        private ILogger<SkillGroupController> _logger;
        private readonly ISkillGroupService _skillGroupService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public SkillGroupController(ILogger<SkillGroupController> logger, ISkillGroupService skillGroupService, IMapper mapper)
        {
            _logger = logger;
            _skillGroupService = skillGroupService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IEnumerable<SkillGroupDto>> Get()
        {
            var skillGroups = await _skillGroupService.GetAll();
            foreach (var skill in skillGroups.SelectMany(x => x.Skills))
            {
                skill.SkillGroup = null;
            }
            return _mapper.Map<IEnumerable<SkillGroupDto>>(skillGroups);
        }
 
        [HttpPost]
        public async Task<SkillGroupDto> Create(CreateUpdateSkillGroupDto model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Invalid model");

            if (await _skillGroupService.IsExistingTitle(model.Title))
                throw new Exception("There is already a skill group with the same title");

            var skillGroup = _mapper.Map<SkillGroup>(model);
            await _skillGroupService.Insert(skillGroup);

            return _mapper.Map<SkillGroupDto>(skillGroup);
        }

        [HttpPut]
        public async Task<SkillGroupDto> Update(CreateUpdateSkillGroupDto model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Invalid model");

            if (!await _skillGroupService.Exists(model.Id))
                 throw new Exception($"No skill group for id: {model.Id} found");

            if (await _skillGroupService.IsExistingTitle(model.Title, model.Id))
                throw new Exception("There is already a skill group with the same title");

            var skillGroup = _mapper.Map<SkillGroup>(model);
            await _skillGroupService.Update(skillGroup);

            return _mapper.Map<SkillGroupDto>(skillGroup);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var skillGroup = await _skillGroupService.GetById(id);
            if (skillGroup == null)
                return BadRequest($"No skill group for id: {id} found");

            await _skillGroupService.Delete(skillGroup);
            return Ok();
        }

        #endregion

    }
}

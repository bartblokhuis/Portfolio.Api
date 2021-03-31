using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Core.Interfaces;
using Portfolio.Domain.Dtos;
using Portfolio.Domain.Models;
using Portfolio.Helpers;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillController : ControllerBase
    {
        #region Fields

        private readonly ILogger<SkillController> _logger;
        private readonly ISkillService _skillService;
        private readonly ISkillGroupService _skillGroupService;
        private readonly IUploadImageHelper _uploadImageHelper;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public SkillController(ILogger<SkillController> logger, ISkillService skillService, ISkillGroupService skillGroupService, IUploadImageHelper uploadImageHelper, IMapper mapper)
        {
            _logger = logger;
            _skillService = skillService;
            _skillGroupService = skillGroupService;
            _uploadImageHelper = uploadImageHelper;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IEnumerable<SkillDto>> Get()
        {
            var skills = await _skillService.GetAll();
            return _mapper.Map<IEnumerable<SkillDto>>(skills);
        }

        [HttpPost]
        public async Task<SkillDto> Create(CreateUpdateSkill model)
        {
            model.Id = 0;

            if (!ModelState.IsValid)
                throw new Exception("Invalid model"); //TODO Better excaption handling

            if (!await _skillGroupService.Exists(model.SkillGroupId))
                throw new Exception("Skill group not found");

            if (await _skillService.IsExistingSkill(model.Name, model.SkillGroupId))
                throw new Exception("Skill with the same name already exists");

            var skill = _mapper.Map<Skill>(model);
            await _skillService.Insert(skill);

            return _mapper.Map<SkillDto>(skill);
        }

        [HttpPut("SaveSkillImage/{skillId}")]
        public async Task<SkillDto> SaveSkillImage(int skillId, IFormFile icon)
        {
            var skill = await _skillService.GetById(skillId);
            if (skill == null)
                throw new Exception("No skill found with the provided id");

            var errorMessage = _uploadImageHelper.ValidateImage(icon);
            if (!string.IsNullOrEmpty(errorMessage))
                throw new Exception(errorMessage);

            skill.IconPath = await _uploadImageHelper.UploadImage(icon);
            await _skillService.Update(skill);
            return _mapper.Map<SkillDto>(skill);
        }

        [HttpPut]
        public async Task<SkillDto> Update(CreateUpdateSkill model)
        {
            if (!ModelState.IsValid)
                throw new Exception("Invalid model"); //TODO Better excaption handling

            var skill = await _skillService.GetById(model.Id);
            if (skill == null)
                throw new Exception("Skill not found");

            if (!await _skillGroupService.Exists(model.SkillGroupId))
                throw new Exception("Skill group not found");

            if (await _skillService.IsExistingSkill(model.Name, model.SkillGroupId, skill))
                throw new Exception("Skill with the same name already exists");

            model.IconPath = skill.IconPath;

            _mapper.Map(model, skill);
            await _skillService.Update(skill);

            return _mapper.Map<SkillDto>(skill);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _skillService.Exists(id))
                return BadRequest("Skill not found");

            await _skillService.Delete(id);

            return Ok();
        }

        #endregion

    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Core.Interfaces.Common;
using Portfolio.Domain.Dtos;
using Portfolio.Domain.Models;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize()]
    public class GeneralSettingsController : ControllerBase
    {
        #region Fields

        private readonly ISettingService<GeneralSettings> _generalSettings;
        private readonly ILogger<GeneralSettingsController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public GeneralSettingsController(ISettingService<GeneralSettings> generalSettings, ILogger<GeneralSettingsController> logger, IMapper mapper)
        {
            _generalSettings = generalSettings;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var settings = await _generalSettings.Get();

            var dto = _mapper.Map<GeneralSettingsDto>(settings);
            dto ??= new GeneralSettingsDto();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Save(GeneralSettingsDto model)
        {
            var originalSettings = await _generalSettings.Get();

            originalSettings ??= new GeneralSettings();
            _mapper.Map(model, originalSettings);

            await _generalSettings.Save(originalSettings);

            return Ok(_mapper.Map<GeneralSettingsDto>(originalSettings));
        }

        #endregion
    }
}

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
    public class SeoSettingsController : ControllerBase
    {
        #region Fields

        private readonly ISettingService<SeoSettings> _seoSettings;
        private readonly ILogger<SeoSettingsController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public SeoSettingsController(ISettingService<SeoSettings> seoSettings, ILogger<SeoSettingsController> logger, IMapper mapper)
        {
            _seoSettings = seoSettings;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var settings = await _seoSettings.Get();

            var dto = _mapper.Map<SeoSettingsDto>(settings);
            dto ??= new SeoSettingsDto();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SeoSettingsDto model)
        {
            var originalSettings = await _seoSettings.Get();

            originalSettings ??= new SeoSettings();
            _mapper.Map(model, originalSettings);

            await _seoSettings.Save(originalSettings);

            return Ok(_mapper.Map<SeoSettingsDto>(originalSettings));
        }
    }
}

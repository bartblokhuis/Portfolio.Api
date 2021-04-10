using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Core.Interfaces;
using Portfolio.Domain.Dtos;
using Portfolio.Domain.Models;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize()]
    public class EmailSettingsController : ControllerBase
    {
        #region Fields

        private readonly ILogger<EmailSettingsController> _logger;
        private readonly IEmailSettingsService _emailSettingsService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public EmailSettingsController(ILogger<EmailSettingsController> logger, IEmailSettingsService emailSettingsService, IMapper mapper)
        {
            _logger = logger;
            _emailSettingsService = emailSettingsService;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var settings = await _emailSettingsService.GetEmailSettings();

            //We don't want to send the password in the get method.
            if(settings != null)
                settings.Password = string.Empty;

            var dto = _mapper.Map<EmailSettingsDto>(settings);
            dto ??= new EmailSettingsDto();

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Save(EmailSettingsDto model)
        {
            var originalSettings = await _emailSettingsService.GetEmailSettings();

            originalSettings ??= new EmailSettings();
            _mapper.Map(model, originalSettings);

            await _emailSettingsService.SaveEmailSettings(originalSettings);

            return Ok(_mapper.Map<EmailSettingsDto>(originalSettings));
        }

        #endregion
    }
}

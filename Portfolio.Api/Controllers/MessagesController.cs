using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Interfaces.Common;
using Portfolio.Domain.Dtos;
using Portfolio.Domain.Enums;
using Portfolio.Domain.Models;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        #region Fields

        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageService _messageService;
        private readonly IWebHelper _webHelper;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public MessagesController(ILogger<MessagesController> logger, IMessageService messageService, IWebHelper webHelper, IMapper mapper)
        {
            _logger = logger;
            _messageService = messageService;
            _webHelper = webHelper;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IEnumerable<MessageDto>> Get()
        {
            var messages = await _messageService.Get();
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        [HttpPost]
        public async Task<MessageDto> Create(MessageDto messageDto)
        {
            var ipAddress = _webHelper.GetCurrentIpAddress();

            if (!await _messageService.IsAllowed(ipAddress))
            {
                //TODO Give proper response
                throw new Exception("Please wait 2 minutes between requests");
            }

            var message = _mapper.Map<Message>(messageDto);
            await _messageService.Create(message);

            return _mapper.Map<MessageDto>(message);
        }

        [HttpPut]
        public async Task<MessageDto> UpdateMessageStatus(int messageId, MessageStatus status)
        {
            var message = await _messageService.GetById(messageId);
            if (message == null)
                throw new Exception("Message not found");

            await _messageService.UpdateMessageStatus(message, status);
            return _mapper.Map<MessageDto>(message);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int messageId)
        {
            var message = await _messageService.GetById(messageId);
            if (message == null)
                return BadRequest("Message not found");

            await _messageService.Delete(message);
            return Ok();
        }
        #endregion
    }
}

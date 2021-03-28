using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Interfaces;
using Portfolio.Core.Interfaces.Common;
using Portfolio.Domain.Enums;
using Portfolio.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Core.Services
{
    public class MessageService : IMessageService
    {
        #region Fields

        private readonly IBaseRepository<Message> _messageRepository;
        private readonly IWebHelper _webHelper;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public MessageService(IWebHelper webHelper, IBaseRepository<Message> messageRepository, IMapper mapper)
        {
            _webHelper = webHelper;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<Message> Create(Message model)
        {
            var ipAddress = _webHelper.GetCurrentIpAddress();

            var message = _mapper.Map<Message>(model);
            message.IpAddress = ipAddress;

            await _messageRepository.InsertAsync(message);
            return message;
        }

        public Task<IEnumerable<Message>> Get()
        {
            return _messageRepository.GetAsync((message) => message.IsDeleted == false);
        }

        public Task<bool> IsAllowed(string ipAddress)
        {
            var minTime = DateTime.UtcNow.AddMinutes(-2);
            return _messageRepository.Table.Where(x => x.IpAddress == ipAddress).AllAsync(x => x.CreatedAtUTC <= minTime);
        }

        public Task<Message> GetById(int messageId)
        {
            return _messageRepository.GetByIdAsync(messageId);
        }

        public Task Delete(Message message)
        {
            return _messageRepository.DeleteAsync(message);
        }

        public Task UpdateMessageStatus(Message message, MessageStatus status)
        {
            message.MessageStatus = status;
            return _messageRepository.UpdateAsync(message);
        }

        #region Utils

        #endregion

        #endregion
    }
}

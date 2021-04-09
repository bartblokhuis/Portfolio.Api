using Portfolio.Core.Interfaces;
using Portfolio.Core.Interfaces.Common;
using Portfolio.Domain.Models;
using System.Threading.Tasks;

namespace Portfolio.Core.Services
{
    public class EmailSettingsService : IEmailSettingsService
    {
        #region Fields

        private readonly IBaseRepository<EmailSettings> _settingsRepository;

        #endregion

        #region Constructor

        public EmailSettingsService(IBaseRepository<EmailSettings> settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        #endregion

        #region Methods

        public Task<EmailSettings> GetEmailSettings()
        {
            return _settingsRepository.FirstAsync();
        }

        public Task SaveEmailSettings(EmailSettings model)
        {
           return (model.Id == 0)?
                _settingsRepository.InsertAsync(model):
                _settingsRepository.UpdateAsync(model);
        }

        #endregion
    }
}

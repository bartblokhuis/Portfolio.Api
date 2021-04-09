using Portfolio.Domain.Models;
using System.Threading.Tasks;

namespace Portfolio.Core.Interfaces
{
    public interface IEmailSettingsService
    {
        Task<EmailSettings> GetEmailSettings();

        Task SaveEmailSettings(EmailSettings model);
    }
}

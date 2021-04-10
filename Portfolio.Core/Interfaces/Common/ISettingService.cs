using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Interfaces.Common
{
    public interface ISettingService<T>
    {
        Task<T> Get();

        Task Save(T settings);
    }
}

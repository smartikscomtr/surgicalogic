using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Contracts.Services
{
    public interface IAppServiceProvider
    {
        TService GetService<TService>();
    }
}

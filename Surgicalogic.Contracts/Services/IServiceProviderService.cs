using System;
using System.Collections.Generic;
using System.Text;

namespace Surgicalogic.Contracts.Services
{
    public interface IServiceProviderService
    {
        TService GetService<TService>();
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Surgicalogic.Contracts.Services
{
    public interface IConnectionProviderService
    {
        IDbConnection GetConnection();
    }
}

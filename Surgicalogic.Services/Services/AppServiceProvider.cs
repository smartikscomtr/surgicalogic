using Microsoft.AspNetCore.Http;
using Surgicalogic.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


namespace Surgicalogic.Services.Services
{
    public class AppServiceProvider : IAppServiceProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppServiceProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public TService GetService<TService>()
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetService<TService>();
        }
    }
}

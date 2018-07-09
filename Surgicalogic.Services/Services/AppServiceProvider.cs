using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Surgicalogic.Contracts.Services;

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
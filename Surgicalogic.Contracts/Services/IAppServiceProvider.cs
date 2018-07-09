namespace Surgicalogic.Contracts.Services
{
    public interface IAppServiceProvider
    {
        TService GetService<TService>();
    }
}
using BuildApps.Core.Mobile.Configurations;
using BuildApps.Core.Mobile.Rest;
using TestProject.Core.Providers;

namespace TestProject.Core.IoC
{
    public class CompositionRoot : BuildApps.Core.Mobile.MvvmCross.IoC.CompositionRoot
    {
        public override void Initialize()
        {
            base.Initialize();
            RegisterProviders();
        }

        protected override void RegisterManagers()
        {

        }

        protected override void RegisterServices()
        {
            Container.RegisterSingleton<IUserSession, UserSession>();
            Container.RegisterSingleton<IHttpClient, HttpClient>();
        }

        protected override void RegisterDependencies()
        {
            base.RegisterDependencies();

            Container.RegisterSingleton<IUserSession, UserSession>();
        }

        private void RegisterProviders()
        {
            Container.RegisterSingleton<IEnvironmentConfigurationProvider, EnvironmentConfigurationProvider>();
        }
    }
}

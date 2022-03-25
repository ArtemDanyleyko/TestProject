using MvvmCross.IoC;
using MvvmCross.ViewModels;
using TestProject.Core.IoC;
using TestProject.Core.ViewModels.Home;

namespace TestProject.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            var compositionRoot = new CompositionRoot();

            base.Initialize();

            compositionRoot.Initialize();

            RegisterAppStart<HomeViewModel>();
        }
    }
}

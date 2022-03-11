using MvvmCross.IoC;
using MvvmCross.ViewModels;
using TestProject.Core.ViewModels.Home;

namespace TestProject.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<HomeViewModel>();
        }
    }
}

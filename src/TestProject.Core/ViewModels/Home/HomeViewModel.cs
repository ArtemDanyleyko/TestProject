using MvvmCross.ViewModels;

namespace TestProject.Core.ViewModels.Home
{
    public class HomeViewModel : MvxViewModel
    {
        private int fillValue;
        public int FillValue
        {
            get => fillValue;
            set => SetProperty(ref fillValue, value);
        }
    }
}

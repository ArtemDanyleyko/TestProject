using BuildApps.Core.Mobile.Rest;
using MvvmCross.ViewModels;

namespace TestProject.Core.ViewModels.Home
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly IHttpClient httpClient;
        private int fillValue;

        public HomeViewModel(IHttpClient httpClient)
        {
            this.httpClient = httpClient;

        }

        public int FillValue
        {
            get => fillValue;
            set => SetProperty(ref fillValue, value);
        }
    }
}

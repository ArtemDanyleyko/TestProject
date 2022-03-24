using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using TestProject.Core.ViewModels.Home;
using TestProject.UI.Extensions;
using TestProject.UI.MvvmCrossBindings;
using Xamarin.Forms;

namespace TestProject.UI.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class HomePage : MvxContentPage<HomeViewModel>
    {
        private readonly ViewBinder<HomeViewModel> viewBinder;

        public HomePage()
        {
            InitializeComponent();

            StarsCanvasView.StrokeColor = Color.Green;
            StarsCanvasView.FillColor = Color.HotPink;

            viewBinder = this.CreateViewBinder<HomeViewModel>(Bind);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void TestClipImage()
        {

        }

        private void Bind()
        {
            using var bindingSet = viewBinder.CreateBindingSet();

            bindingSet.Bind(StarsSlider).For(v => v.Value).To(vm => vm.FillValue);
            bindingSet.Bind(TitleLabel).For(v => v.Text).To(vm => vm.FillValue);
            bindingSet.Bind(StarsCanvasView).For(v => v.Value).To(vm => vm.FillValue);
        }
    }
}

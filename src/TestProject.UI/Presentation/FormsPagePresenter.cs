using MvvmCross.Forms.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using TestProject.UI.Presentation.Attributes;

namespace TestProject.UI.Presentation
{
    public class FormsPagePresenter : MvxFormsPagePresenter
    {
        public FormsPagePresenter(IMvxFormsViewPresenter platformPresenter)
              : base(platformPresenter)
        {
        }

        private IPopupNavigation popupNavigation;
        private IPopupNavigation PopupNavigation => popupNavigation ??= Rg.Plugins.Popup.Services.PopupNavigation.Instance;

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            return base.CreatePresentationAttribute(viewModelType, viewType);
        }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();
        }

        private async Task<bool> ShowPopupPageAsync(Type view, PopupPresentationAttribute attribute, MvxViewModelRequest request)
        {
            return true;
        }

        private async Task<bool> ClosePopupPageAsync(IMvxViewModel _, PopupPresentationAttribute __)
        {
            await PopupNavigation.PopAsync();
            return true;
        }
    }
}

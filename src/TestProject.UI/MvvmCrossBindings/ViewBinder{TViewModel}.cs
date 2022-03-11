using MvvmCross.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.WeakSubscription;
using System;
using Xamarin.Forms;

namespace TestProject.UI.MvvmCrossBindings
{
    public class ViewBinder<TViewModel> : IMvxBindingContextOwner
         where TViewModel : IMvxNotifyPropertyChanged
    {
        private readonly WeakReference<VisualElement> visualElementWeakReference;
        private readonly Action bindAction;
        private readonly Action<TViewModel> viewModelChangedAction;

        public ViewBinder(
            VisualElement visualElement,
            Action bindAction,
            Action<TViewModel> viewModelChangedAction = null)
        {
            visualElement.WeakSubscribe(nameof(visualElement.BindingContextChanged), OnVisualElementBindingContextChanged);
            visualElementWeakReference = new WeakReference<VisualElement>(visualElement);

            this.bindAction = bindAction;
            this.viewModelChangedAction = viewModelChangedAction;
        }

        public IMvxBindingContext BindingContext { get; set; } = new MvxBindingContext();

        private TViewModel viewModel;
        public TViewModel ViewModel
        {
            get => viewModel;
            private set
            {
                if (ReferenceEquals(viewModel, value))
                {
                    return;
                }

                viewModel = value;

                BindingContext.DataContext = viewModel;
                BindingContext.ClearAllBindings();
                viewModelChangedAction?.Invoke(viewModel);

                if (viewModel != null)
                {
                    bindAction();
                }
            }
        }

        public MvxFluentBindingDescriptionSet<ViewBinder<TViewModel>, TViewModel> CreateBindingSet() =>
            this.CreateBindingSet<ViewBinder<TViewModel>, TViewModel>();

        public MvxFluentBindingDescriptionSet<ViewBinder<TViewModel>, TViewModel> CreateBindingSet(string clearBindingKey) =>
            this.CreateBindingSet<ViewBinder<TViewModel>, TViewModel>(clearBindingKey);

        private void OnVisualElementBindingContextChanged(object _, EventArgs __)
        {
            if (!visualElementWeakReference.TryGetTarget(out var visualElement))
            {
                return;
            }

            ViewModel = visualElement.BindingContext switch
            {
                TViewModel viewModel => viewModel,
                _ => default
            };
        }
    }
}

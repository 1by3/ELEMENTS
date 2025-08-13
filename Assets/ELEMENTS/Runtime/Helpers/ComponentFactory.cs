using System;
using ELEMENTS.MVVM;

namespace ELEMENTS.Helpers
{
    public static class ComponentFactory
    {
        public static (TViewModel, TView) CreateComponent<TViewModel, TView>(Action<TViewModel> configure = null)
            where TViewModel : ViewModel
            where TView : View<TViewModel>
        {
            var viewModel = (TViewModel)Activator.CreateInstance(typeof(TViewModel), Array.Empty<object>());
            // ReSharper disable once RedundantExplicitParamsArrayCreation
            var view = (TView)Activator.CreateInstance(typeof(TView), new object[] { viewModel });

            configure?.Invoke(viewModel);

            return (viewModel, view);
        }
    }
}
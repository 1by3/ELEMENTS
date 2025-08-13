using System;
using ELEMENTS.Scripts.Helpers;
using ELEMENTS.Scripts.MVVM;
using UnityEngine.UIElements;

namespace ELEMENTS.Scripts.Extensions
{
    public static class VisualElementExtensions
    {
        public static (TViewModel, TView) RenderComponent<TViewModel, TView>(this VisualElement visualElement, Action<TViewModel> configure = null)
            where TViewModel : ViewModel
            where TView : View<TViewModel>
        {
           var (viewModel, view) = ComponentFactory.CreateComponent<TViewModel, TView>(configure);
            visualElement.Clear();
            visualElement.Add(view.BuildVisualElement());
            return (viewModel, view);
        }
    }
}
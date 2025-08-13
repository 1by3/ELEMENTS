using System;
using ELEMENTS.Elements;
using ELEMENTS.Helpers;
using UnityEngine.UIElements;

namespace ELEMENTS.MVVM
{
    public abstract class View<TViewModel> where TViewModel : ViewModel
    {
        protected readonly TViewModel ViewModel;

        public View(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public static IElement Component<TComponentViewModel, TComponentView>(Action<TComponentViewModel> configure = null)
            where TComponentViewModel : ViewModel
            where TComponentView : View<TComponentViewModel>
        {
            var (_, view) = ComponentFactory.CreateComponent<TComponentViewModel, TComponentView>(configure);
            return view.Render();
        }

        public VisualElement BuildVisualElement()
        {
            return Render().BuildVisualElement();
        }

        protected abstract IElement Render();
    }
}

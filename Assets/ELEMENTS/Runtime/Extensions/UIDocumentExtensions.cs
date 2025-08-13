using ELEMENTS.MVVM;
using UnityEngine.UIElements;

namespace ELEMENTS.Extensions
{
    public static class UIDocumentExtensions
    {
        public static (TViewModel, TView) RenderComponent<TViewModel, TView>(this UIDocument uiDocument)
            where TViewModel : ViewModel
            where TView : View<TViewModel>
        {
            return uiDocument.rootVisualElement.RenderComponent<TViewModel, TView>();
        }
    }
}
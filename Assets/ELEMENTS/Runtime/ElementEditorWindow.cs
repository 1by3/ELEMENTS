using ELEMENTS.MVVM;
using ELEMENTS.Extensions;
using UnityEditor;

namespace ELEMENTS.Editor
{
    public class ElementEditorWindow<TViewModel, TView> : EditorWindow
        where TViewModel : ViewModel
        where TView : View<TViewModel>
    {
        protected void CreateGUI()
        {
            rootVisualElement.RenderComponent<TViewModel, TView>();
        }
    }
}
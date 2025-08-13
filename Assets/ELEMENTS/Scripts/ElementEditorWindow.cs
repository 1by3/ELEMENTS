using ELEMENTS.Scripts.Extensions;
using ELEMENTS.Scripts.MVVM;
using UnityEditor;

namespace ELEMENTS.Scripts.Editor
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
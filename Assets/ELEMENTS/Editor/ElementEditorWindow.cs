using ELEMENTS.Extensions;
using UnityEditor;

namespace ELEMENTS.Editor
{
    #if UNITY_EDITOR

    public class ElementEditorWindow<T> : EditorWindow where T : Component, new()
    {
        protected void CreateGUI()
        {
            rootVisualElement.RenderComponent<T>();
        }
    }

    #endif
}

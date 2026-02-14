using ELEMENTS.Elements;
using UnityEditor;
using UnityEngine.UIElements;

namespace ELEMENTS.Editor
{
    public static class EditorWindowExtensions
    {
        public static void RenderElement(this EditorWindow editorWindow, IElement rootElement, bool additive = false)
        {
            if (!additive) editorWindow.rootVisualElement.Clear();
            editorWindow.rootVisualElement.Add(rootElement.BuildVisualElement());
        }

        public static void RenderElement(this EditorWindow editorWindow, Component rootComponent, bool additive = false)
        {
            if (!additive) editorWindow.rootVisualElement.Clear();
            editorWindow.rootVisualElement.Add(rootComponent.BuildVisualElement());
        }
    }
}
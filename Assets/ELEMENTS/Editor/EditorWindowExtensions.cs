using System;
using ELEMENTS.Elements;
using UnityEditor;
using UnityEngine.UIElements;

namespace ELEMENTS.Editor
{
    public static class EditorWindowExtensions
    {
        public static IDisposable RenderElement(this EditorWindow editorWindow, IElement rootElement, bool additive = false)
        {
            if (!additive) editorWindow.rootVisualElement.Clear();
            editorWindow.rootVisualElement.Add(rootElement.BuildVisualElement());
            return rootElement as IDisposable;
        }

        public static IDisposable RenderElement(this EditorWindow editorWindow, Component rootComponent, bool additive = false)
        {
            if (!additive) editorWindow.rootVisualElement.Clear();
            editorWindow.rootVisualElement.Add(rootComponent.BuildVisualElement());
            return rootComponent;
        }
    }
}
using System;
using ELEMENTS.Elements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS.Extensions
{
    public static class UIDocumentExtensions
    {
        public static void RenderElement(this UIDocument uiDocument, IElement rootElement, bool additive = false)
        {
            if (!additive) uiDocument.rootVisualElement.Clear();
            uiDocument.rootVisualElement.Add(rootElement.BuildVisualElement());
        }

        public static void RenderElement(this UIDocument uiDocument, Component rootComponent, bool additive = false)
        {
            if (!additive) uiDocument.rootVisualElement.Clear();
            uiDocument.rootVisualElement.Add(rootComponent.BuildVisualElement());
        }

        public static void AddStyleSheet(this UIDocument uiDocument, string resourcePath)
        {
            var styleSheet = Resources.Load<StyleSheet>(resourcePath);
            if (styleSheet == null) throw new Exception($"StyleSheet not found at path: {resourcePath}");
            uiDocument.rootVisualElement.styleSheets.Add(styleSheet);
        }

        public static void AddStyleSheet(this UIDocument uiDocument, StyleSheet styleSheet)
        {
            uiDocument.rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}

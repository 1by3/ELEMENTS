using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS.Extensions
{
    public static class UIDocumentExtensions
    {
        public static T RenderComponent<T>(this UIDocument uiDocument, Action<T> configure = null)
            where T : Component, new()
        {
            return uiDocument.rootVisualElement.RenderComponent<T>(configure);
        }

        public static void AddStyleSheet(this UIDocument uiDocument, string resourcePath)
        {
            var styleSheet = Resources.Load<StyleSheet>(resourcePath);
            if (styleSheet == null) throw new System.Exception($"StyleSheet not found at path: {resourcePath}");
            uiDocument.rootVisualElement.styleSheets.Add(styleSheet);
        }

        public static void AddStyleSheet(this UIDocument uiDocument, StyleSheet styleSheet)
        {
            uiDocument.rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}

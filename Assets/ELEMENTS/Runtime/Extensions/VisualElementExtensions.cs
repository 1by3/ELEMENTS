using System;
using ELEMENTS.Elements;
using ELEMENTS.Helpers;
using UnityEngine.UIElements;

namespace ELEMENTS.Extensions
{
    public static class VisualElementExtensions
    {
        public static IDisposable RenderElement(this VisualElement visualElement, IElement rootElement, bool additive = false)
        {
            if (!additive) visualElement.Clear();
            visualElement.Add(rootElement.BuildVisualElement());
            return rootElement as IDisposable;
        }

        public static IDisposable RenderElement(this VisualElement visualElement, Component rootComponent, bool additive = false)
        {
            if (!additive) visualElement.Clear();
            visualElement.Add(rootComponent.BuildVisualElement());
            return rootComponent;
        }
    }
}

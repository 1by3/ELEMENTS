using System;
using ELEMENTS.Helpers;
using UnityEngine.UIElements;

namespace ELEMENTS.Extensions
{
    public static class VisualElementExtensions
    {
        public static T RenderComponent<T>(this VisualElement visualElement, Action<T> configure = null)
            where T : Component, new()
        {
            var component = ComponentFactory.Create<T>(configure);
            visualElement.Clear();
            visualElement.Add(component.BuildVisualElement());
            return component;
        }
    }
}

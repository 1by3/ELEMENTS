using System;

namespace ELEMENTS.Helpers
{
    public static class ComponentFactory
    {
        public static T Create<T>(Action<T> configure = null) where T : Component, new()
        {
            var component = new T();
            configure?.Invoke(component);
            return component;
        }
    }
}

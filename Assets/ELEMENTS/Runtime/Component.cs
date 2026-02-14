using System;
using ELEMENTS.Elements;
using UnityEngine.UIElements;

namespace ELEMENTS
{
    public abstract class Component : IElement, IDisposable
    {
        private IElement _renderedElement;

        protected abstract IElement Render();

        public VisualElement BuildVisualElement()
        {
            _renderedElement = Render();
            return _renderedElement.BuildVisualElement();
        }

        public virtual void Dispose()
        {
            if (_renderedElement is IDisposable disposable)
                disposable.Dispose();
            _renderedElement = null;
        }
    }
}

using System;
using System.Collections.Generic;
using ELEMENTS.Elements;
using UnityEngine.UIElements;

namespace ELEMENTS
{
    public abstract class Component : IElement, IDisposable
    {
        private IElement _renderedElement;
        private List<IDisposable> _updateRegistrations;

        protected abstract IElement Render();

        public VisualElement BuildVisualElement()
        {
            _renderedElement = Render();

            if (this is IUpdatable updatable)
            {
                _updateRegistrations ??= new List<IDisposable>();
                _updateRegistrations.Add(ElementPortal.Register(updatable));
            }

            if (this is IFixedUpdatable fixedUpdatable)
            {
                _updateRegistrations ??= new List<IDisposable>();
                _updateRegistrations.Add(ElementPortal.Register(fixedUpdatable));
            }

            return _renderedElement.BuildVisualElement();
        }

        public virtual void Dispose()
        {
            if (_updateRegistrations != null)
            {
                foreach (var reg in _updateRegistrations)
                    reg.Dispose();
                _updateRegistrations.Clear();
            }

            if (_renderedElement is IDisposable disposable)
                disposable.Dispose();
            _renderedElement = null;
        }
    }
}

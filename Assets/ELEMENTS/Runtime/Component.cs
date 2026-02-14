using System;
using System.Collections.Generic;
using ELEMENTS.Elements;
using UnityEngine.UIElements;

namespace ELEMENTS
{
    public abstract class Component : IElement, IDisposable
    {
        protected readonly List<IDisposable> Disposables = new();
        protected IElement RenderedElement;

        private List<IDisposable> updateRegistrations;

        protected abstract IElement Render();

        public VisualElement BuildVisualElement()
        {
            RenderedElement = Render();

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (this is IUpdatable updatable)
            {
                updateRegistrations ??= new List<IDisposable>();
                updateRegistrations.Add(ElementPortal.Register(updatable));
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (this is IFixedUpdatable fixedUpdatable)
            {
                updateRegistrations ??= new List<IDisposable>();
                updateRegistrations.Add(ElementPortal.Register(fixedUpdatable));
            }

            return RenderedElement.BuildVisualElement();
        }

        public virtual void Dispose()
        {
            foreach (var d in Disposables)
            {
                d.Dispose();
            }

            Disposables.Clear();

            if (updateRegistrations != null)
            {
                foreach (var reg in updateRegistrations)
                    reg.Dispose();
                updateRegistrations.Clear();
            }

            if (RenderedElement is IDisposable disposable)
                disposable.Dispose();
            RenderedElement = null;
        }
    }
}

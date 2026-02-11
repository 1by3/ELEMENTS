using ELEMENTS.Elements;
using UnityEngine.UIElements;

namespace ELEMENTS
{
    public abstract class Component : IElement
    {
        protected abstract IElement Render();

        public VisualElement BuildVisualElement() => Render().BuildVisualElement();
    }
}

using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class VerticalGroup<T> : Group<T> where T : VerticalGroup<T>
    {
        public VerticalGroup(params IElement[] children)
            : base(children)
        {
            VisualElement.style.flexDirection = FlexDirection.Column;
            VisualElement.AddToClassList("flex-col");
        }

        public T Reverse(bool reverse = true)
        {
            VisualElement.style.flexDirection = reverse ? FlexDirection.ColumnReverse : FlexDirection.Column;
            return (T)this;
        }
    }

    public class VerticalGroup : VerticalGroup<VerticalGroup>
    {
        public VerticalGroup(params IElement[] children) : base(children) { }
    }
}
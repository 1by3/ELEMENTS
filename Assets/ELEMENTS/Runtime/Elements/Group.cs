using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public abstract class Group<T> : BaseElement<T> where T : Group<T>
    {
        protected readonly List<IElement> Children;

        public Group(params IElement[] children)
        {
            Children = new List<IElement>(children);
        }

        public T SetChildren(params IElement[] children)
        {
            Children.Clear();
            Children.AddRange(children);
            VisualElement.Clear();
            BuildVisualElement();

            return (T)this;
        }

        public T BindChildren<TChild>(Observable<TChild[]> array, Func<TChild, IElement> converter)
        {
            Disposables.Add(array.Subscribe(newItems =>
            {
                var childElements = newItems.Select(converter).ToArray();
                SetChildren(childElements);
            }));

            return (T)this;
        }

        public override VisualElement BuildVisualElement()
        {
            VisualElement.Clear();

            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                var childElement = child.BuildVisualElement();
                if (childElement == null) continue;
                if (index == 0) childElement.AddToClassList("first-child");
                if (index == Children.Count - 1) childElement.AddToClassList("last-child");
                childElement.AddToClassList(index % 2 == 0 ? "even-child" : "odd-child");
                VisualElement.Add(childElement);
            }

            return base.BuildVisualElement();
        }

        public T Flex(int flexGrow, int flexShrink = 0)
        {
            VisualElement.style.flexGrow = flexGrow;
            VisualElement.style.flexShrink = flexShrink;
            return (T)this;
        }

        public T JustifyContent(Justify justify)
        {
            VisualElement.style.justifyContent = justify;
            return (T)this;
        }

        public T AlignItems(Align align)
        {
            VisualElement.style.alignItems = align;
            return (T)this;
        }
    }
}
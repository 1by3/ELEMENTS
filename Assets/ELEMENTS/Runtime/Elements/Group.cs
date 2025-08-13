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

        public T BindChildren<TChild>(Observable<TChild[]> array, Func<TChild, IElement> converter)
        {
            Disposables.Add(array.Subscribe(newItems =>
            {
                var childElements = newItems.Select(converter).ToArray();
                Children.Clear();
                Children.AddRange(childElements);
                VisualElement.Clear();
                BuildVisualElement();
            }));

            return (T)this;
        }

        public override VisualElement BuildVisualElement()
        {
            VisualElement.Clear();

            foreach (var child in Children)
            {
                VisualElement.Add(child.BuildVisualElement());
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
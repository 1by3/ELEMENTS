namespace ELEMENTS.Scripts.Elements
{
    public class ScrollView<T> : Group<T> where T : ScrollView<T>
    {
        public ScrollView(params IElement[] children) : base(children)
        {
            VisualElement = new UnityEngine.UIElements.ScrollView();
        }
    }

    public class ScrollView : ScrollView<ScrollView>
    {
        public ScrollView() : base()
        {
        }

        public ScrollView(params IElement[] children) : base(children)
        {
        }
    }
}
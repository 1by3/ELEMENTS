namespace ELEMENTS.Elements
{
    public class Button<T> : Group<T> where T : Button<T>
    {
        public Button(params IElement[] children) : base(children)
        {
            VisualElement = new UnityEngine.UIElements.Button();
        }
    }

    public class Button : Button<Button>
    {
        public Button(params IElement[] children) : base(children) { }
    }
}
using R3;

namespace ELEMENTS.Elements
{
    public class Label<T> : BaseElement<T> where T : Label<T>
    {
        public Label(string text)
        {
            VisualElement = new UnityEngine.UIElements.Label(text);
        }

        public Label() : this("")
        {
        }

        public Label(Observable<string> text) : this()
        {
            BindText(text);
        }

        public T Text(string text)
        {
            ((UnityEngine.UIElements.Label)VisualElement).text = text;
            return (T)this;
        }

        public string GetText()
        {
            return ((UnityEngine.UIElements.Label)VisualElement).text;
        }

        public T BindText(Observable<string> text)
        {
            Disposables.Add(text.Subscribe(nv => Text(nv)));
            return (T)this;
        }

    }

    public class Label : Label<Label>
    {
        public Label(string text) : base(text) { }
        public Label() : base("") { }
        public Label(Observable<string> text) : base(text) { }
    }
}
using R3;

namespace ELEMENTS.Elements
{
    public class TableCaption<T> : BaseElement<T> where T : TableCaption<T>
    {
        public TableCaption(string text)
        {
            VisualElement = new UnityEngine.UIElements.Label(text);
            VisualElement.AddToClassList("table-caption");
        }

        public TableCaption() : this("")
        {
        }

        public TableCaption(Observable<string> text) : this()
        {
            BindText(text);
        }

        public T Text(string text)
        {
            ((UnityEngine.UIElements.Label)VisualElement).text = text;
            return (T)(object)this;
        }

        public string GetText()
        {
            return ((UnityEngine.UIElements.Label)VisualElement).text;
        }

        public T BindText(Observable<string> text)
        {
            Disposables.Add(text.Subscribe(nv => Text(nv)));
            return (T)(object)this;
        }
    }

    public class TableCaption : TableCaption<TableCaption>
    {
        public TableCaption(string text) : base(text) { }
        public TableCaption() : base("") { }
        public TableCaption(Observable<string> text) : base(text) { }
    }
}

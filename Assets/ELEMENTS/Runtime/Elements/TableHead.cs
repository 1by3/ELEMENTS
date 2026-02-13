using R3;

namespace ELEMENTS.Elements
{
    public class TableHead<T> : Group<T> where T : TableHead<T>
    {
        public TableHead(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("table-head");
            VisualElement.style.flexGrow = 1;
            VisualElement.style.flexBasis = 0;
        }

        public TableHead(string text) : this(new Label(text))
        {
        }

        public TableHead(Observable<string> text) : this(new Label(text))
        {
        }
    }

    public class TableHead : TableHead<TableHead>
    {
        public TableHead(params IElement[] children) : base(children) { }
        public TableHead(string text) : base(text) { }
        public TableHead(Observable<string> text) : base(text) { }
    }
}

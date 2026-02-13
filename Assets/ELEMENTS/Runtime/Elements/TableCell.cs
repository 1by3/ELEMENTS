using R3;

namespace ELEMENTS.Elements
{
    public class TableCell<T> : Group<T> where T : TableCell<T>
    {
        public TableCell(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("table-cell");
            VisualElement.style.flexGrow = 1;
            VisualElement.style.flexBasis = 0;
        }

        public TableCell(string text) : this(new Label(text))
        {
        }

        public TableCell(Observable<string> text) : this(new Label(text))
        {
        }
    }

    public class TableCell : TableCell<TableCell>
    {
        public TableCell(params IElement[] children) : base(children) { }
        public TableCell(string text) : base(text) { }
        public TableCell(Observable<string> text) : base(text) { }
    }
}

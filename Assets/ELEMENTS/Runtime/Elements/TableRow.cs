namespace ELEMENTS.Elements
{
    public class TableRow<T> : HorizontalGroup<T> where T : TableRow<T>
    {
        public TableRow(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("table-row");
        }
    }

    public class TableRow : TableRow<TableRow>
    {
        public TableRow(params IElement[] children) : base(children) { }
    }
}

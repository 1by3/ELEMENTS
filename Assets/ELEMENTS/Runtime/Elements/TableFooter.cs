namespace ELEMENTS.Elements
{
    public class TableFooter<T> : VerticalGroup<T> where T : TableFooter<T>
    {
        public TableFooter(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("table-footer");
        }
    }

    public class TableFooter : TableFooter<TableFooter>
    {
        public TableFooter(params IElement[] children) : base(children) { }
    }
}

namespace ELEMENTS.Elements
{
    public class TableBody<T> : VerticalGroup<T> where T : TableBody<T>
    {
        public TableBody(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("table-body");
        }
    }

    public class TableBody : TableBody<TableBody>
    {
        public TableBody(params IElement[] children) : base(children) { }
    }
}

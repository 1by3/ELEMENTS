namespace ELEMENTS.Elements
{
    public class TableHeader<T> : VerticalGroup<T> where T : TableHeader<T>
    {
        public TableHeader(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("table-header");
        }
    }

    public class TableHeader : TableHeader<TableHeader>
    {
        public TableHeader(params IElement[] children) : base(children) { }
    }
}

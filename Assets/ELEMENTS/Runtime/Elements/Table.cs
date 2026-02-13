namespace ELEMENTS.Elements
{
    public class Table<T> : VerticalGroup<T> where T : Table<T>
    {
        public Table(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("table");
        }
    }

    public class Table : Table<Table>
    {
        public Table(params IElement[] children) : base(children) { }
    }
}

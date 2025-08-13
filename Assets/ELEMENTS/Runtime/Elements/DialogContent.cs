namespace ELEMENTS.Scripts.Elements
{
    public class DialogContent<T> : VerticalGroup<T> where T : DialogContent<T>
    {
        public DialogContent(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("elements-dialog-content");
        }
    }

    public class DialogContent : DialogContent<DialogContent>
    {
        public DialogContent(params IElement[] children) : base(children)
        {
        }
    }
}
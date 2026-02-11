namespace ELEMENTS.Elements
{
    public class Alert<T> : Dialog<T> where T : Alert<T>
    {
        public Alert(string title, string message) : this(ElementPortal.Current, title, message) { }

        public Alert(ElementPortal portal, string title, string message) : base(portal)
        {
            Children.Add(
                new DialogContent(
                    new Label(title).ClassName("title"),
                    new Label(message).ClassName("message"),
                    new Button(new Label("Dismiss")).ClassName("pill").OnClick(_ => Close())
                )
            );

            ClassName("elements-alert");
        }
    }

    public class Alert : Alert<Alert>
    {
        public Alert(string title, string message) : base(title, message) { }
        public Alert(ElementPortal portal, string title, string message) : base(portal, title, message) { }
    }
}
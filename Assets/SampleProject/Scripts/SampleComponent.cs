using ELEMENTS.Elements;
using ELEMENTS.Extensions;
using R3;
using Component = ELEMENTS.Component;
using ContextMenu = ELEMENTS.Elements.ContextMenu;

namespace SampleProject.Scripts
{
    public class SampleComponent : Component
    {
        public readonly ReactiveProperty<int> Count = new(0);
        public readonly ReactiveProperty<bool> AlertOpen = new(false);
        public readonly ReactiveProperty<bool> DarkMode = new(false);
        public readonly ReactiveProperty<bool> Notifications = new(true);

        public void OpenAlert() => AlertOpen.Value = true;
        public void CloseAlert() => AlertOpen.Value = false;
        public void Increment() => Count.Value++;
        public void Decrement() => Count.Value--;
        public void Reset() => Count.Value = 0;
        public void Double() => Count.Value *= 2;

        protected override IElement Render()
        {
            // Dropdown menu example
            var optionsMenu = new ContextMenu(
                new MenuItem("Increment", () => Increment()),
                new MenuItem("Decrement", () => Decrement()),
                new MenuDivider(),
                new MenuItem("Show Alert", () => OpenAlert())
            );

            // Right-click context menu on image
            var contextMenu = new ContextMenu(
                new MenuItem("Reset Count", () => Reset()),
                new MenuItem("Double Count", () => Double())
            );

            // Background right-click menu (works anywhere on the main view)
            var backgroundMenu = new ContextMenu(
                new MenuItem("Increment", () => Increment()),
                new MenuItem("Decrement", () => Decrement()),
                new MenuDivider(),
                new MenuItem("Reset", () => Reset())
            );

            return new VerticalGroup(
                new Alert("Hello world", "how are you?")
                    .BindOpen(AlertOpen)
                    .OnClose(() => CloseAlert()),

                new Image("SampleProject/logo_wide_color")
                    .ClassName("logo")
                    .WithRightClickMenu(contextMenu),

                new Label(Count.Select(count => $"Count: {count}"))
                    .ClassName("count-label"),

                new HorizontalGroup(
                    new Button(new Label("Increment"))
                        .OnClick(_ => Increment())
                        .ClassName("increment-button"),
                    new Button(new Label("Decrement"))
                        .OnClick(_ => Decrement())
                        .ClassName("decrement-button"),
                    new Button(new Label("Show alert"))
                        .OnClick(_ => OpenAlert())
                        .ClassName("alert-button"),
                    new Button(new Label("Options"))
                        .WithContextMenu(optionsMenu)
                        .ClassName("options-button")
                ).ClassName("gap-2"),

                new VerticalGroup(
                new Table(
                    new TableCaption("A list of your recent invoices."),
                    new TableHeader(
                        new TableRow(
                            new TableHead(""),
                            new TableHead("Invoice"),
                            new TableHead("Status"),
                            new TableHead("Method"),
                            new TableHead("Amount")
                        )
                    ),
                    new TableBody(
                        new TableRow(
                            new TableCell(new Checkbox().Value(true)),
                            new TableCell("INV001"),
                            new TableCell("Paid"),
                            new TableCell("Credit Card"),
                            new TableCell("$250.00")
                        ),
                        new TableRow(
                            new TableCell(new Checkbox()),
                            new TableCell("INV002"),
                            new TableCell("Pending"),
                            new TableCell("PayPal"),
                            new TableCell("$150.00")
                        ),
                        new TableRow(
                            new TableCell(new Checkbox()),
                            new TableCell("INV003"),
                            new TableCell("Unpaid"),
                            new TableCell("Bank Transfer"),
                            new TableCell("$350.00")
                        )
                    ),
                    new TableFooter(
                        new TableRow(
                            new TableCell("Total"),
                            new TableCell(""),
                            new TableCell(""),
                            new TableCell("$750.00")
                        )
                    )
                )
                ).ClassName("table-container"),

                new HorizontalGroup(
                    new Checkbox(DarkMode)
                        .Label("Dark mode"),
                    new Checkbox(Notifications)
                        .Label("Enable notifications")
                ).ClassName("gap-4")
            ).ClassName("main-view").StyleSheet("SampleProject/SampleStyles").WithRightClickMenu(backgroundMenu);
        }
    }
}

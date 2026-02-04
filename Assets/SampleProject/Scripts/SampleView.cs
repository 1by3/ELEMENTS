using ELEMENTS;
using ELEMENTS.Elements;
using ELEMENTS.Extensions;
using ELEMENTS.MVVM;
using R3;
using UnityEngine;
using ContextMenu = ELEMENTS.Elements.ContextMenu;

namespace SampleProject.Scripts
{
    public class SampleView : View<SampleViewModel>
    {
        public SampleView(SampleViewModel viewModel) : base(viewModel)
        {
        }

        protected override IElement Render()
        {
            var portal = Object.FindFirstObjectByType<ElementPortal>();

            // Dropdown menu example
            var optionsMenu = new ContextMenu(portal,
                new MenuItem("Increment", () => ViewModel.Increment()),
                new MenuItem("Decrement", () => ViewModel.Decrement()),
                new MenuDivider(),
                new MenuItem("Show Alert", () => ViewModel.OpenAlert())
            );

            // Right-click context menu on image
            var contextMenu = new ContextMenu(portal,
                new MenuItem("Reset Count", () => ViewModel.Reset()),
                new MenuItem("Double Count", () => ViewModel.Double())
            );

            // Background right-click menu (works anywhere on the main view)
            var backgroundMenu = new ContextMenu(portal,
                new MenuItem("Increment", () => ViewModel.Increment()),
                new MenuItem("Decrement", () => ViewModel.Decrement()),
                new MenuDivider(),
                new MenuItem("Reset", () => ViewModel.Reset())
            );

            return new VerticalGroup(
                new Alert(portal, "Hello world", "how are you?")
                    .BindOpen(ViewModel.AlertOpen)
                    .OnClose(() => ViewModel.CloseAlert()),

                new Image("SampleProject/logo_wide_color")
                    .ClassName("logo")
                    .WithRightClickMenu(contextMenu),

                new Label(ViewModel.Count.Select(count => $"Count: {count}"))
                    .ClassName("count-label"),

                new HorizontalGroup(
                    new Button(new Label("Increment"))
                        .OnClick(_ => ViewModel.Increment())
                        .ClassName("increment-button"),
                    new Button(new Label("Decrement"))
                        .OnClick(_ => ViewModel.Decrement())
                        .ClassName("decrement-button"),
                    new Button(new Label("Show alert"))
                        .OnClick(_ => ViewModel.OpenAlert())
                        .ClassName("alert-button"),
                    new Button(new Label("Options"))
                        .WithContextMenu(optionsMenu)
                        .ClassName("options-button")
                )
            ).ClassName("main-view").StyleSheet("SampleProject/SampleStyles").WithRightClickMenu(backgroundMenu);
        }
    }
}
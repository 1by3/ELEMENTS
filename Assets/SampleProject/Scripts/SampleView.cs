using ELEMENTS;
using ELEMENTS.Elements;
using ELEMENTS.MVVM;
using R3;
using UnityEngine;

namespace SampleProject.Scripts
{
    public class SampleView : View<SampleViewModel>
    {
        public SampleView(SampleViewModel viewModel) : base(viewModel)
        {
        }

        protected override IElement Render()
        {
            return new VerticalGroup(
                new Alert(Object.FindFirstObjectByType<ElementPortal>(), "Hello world", "how are you?")
                    .BindOpen(ViewModel.AlertOpen)
                    .OnClose(() => ViewModel.CloseAlert()),

                new Image("SampleProject/logo_wide_color")
                    .ClassName("logo"),

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
                        .ClassName("alert-button")
                )
            ).ClassName("main-view").StyleSheet("SampleProject/SampleStyles");
        }
    }
}
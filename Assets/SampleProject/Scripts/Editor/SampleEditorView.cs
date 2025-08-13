using ELEMENTS.Scripts.Elements;
using ELEMENTS.Scripts.MVVM;
using R3;

namespace SampleProject.Scripts.Editor
{
    public class SampleEditorView : View<SampleViewModel>
    {
        public SampleEditorView(SampleViewModel viewModel) : base(viewModel)
        {
        }

        protected override IElement Render()
        {
            return new VerticalGroup(
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
                        .ClassName("decrement-button")
                )
            ).ClassName("main-view").StyleSheet("SampleProject/SampleEditorStyles");
        }
    }
}
using ELEMENTS;
using ELEMENTS.Elements;
using R3;

namespace SampleProject.Scripts.Editor
{
    public class SampleEditorComponent : Component
    {
        public readonly ReactiveProperty<int> Count = new(0);

        public void Increment() => Count.Value++;
        public void Decrement() => Count.Value--;

        protected override IElement Render()
        {
            return new VerticalGroup(
                new Image("SampleProject/logo_wide_color")
                    .ClassName("logo"),

                new Label(Count.Select(count => $"Count: {count}"))
                    .ClassName("count-label"),

                new HorizontalGroup(
                    new Button(new Label("Increment"))
                        .OnClick(_ => Increment())
                        .ClassName("increment-button"),
                    new Button(new Label("Decrement"))
                        .OnClick(_ => Decrement())
                        .ClassName("decrement-button")
                )
            ).ClassName("main-view").StyleSheet("SampleProject/SampleEditorStyles");
        }
    }
}

using ELEMENTS;

namespace SampleProject.Scripts
{
    public class SampleElementPortal : ElementPortal
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            StyleSheet("ELEMENTS/DefaultStyles");
            RenderComponent<SampleViewModel, SampleView>();
        }
    }
}
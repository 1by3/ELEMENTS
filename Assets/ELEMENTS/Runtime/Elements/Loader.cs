using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class Loader<T> : Image<T> where T : Loader<T>
    {
        private readonly IVisualElementScheduledItem scheduler;

        public Loader() : base("ELEMENTS/loader-black-24")
        {
            VisualElement.AddToClassList("elements-loader");
            scheduler = VisualElement.schedule.Execute(TickAnimation).Every(1).StartingIn(0);
        }

        private void TickAnimation()
        {
            var degreesPerFrame = 360f;
            var angle = VisualElement.style.rotate.value.angle.value + degreesPerFrame;
            if (angle >= 360f) angle -= 360f;
            VisualElement.style.rotate = new Rotate(angle);
        }

        public override void Dispose()
        {
            base.Dispose();
            scheduler.Pause();
        }
    }

    public class Loader : Loader<Loader>
    {
    }
}
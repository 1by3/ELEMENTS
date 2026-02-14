using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class Loader<T> : Image<T> where T : Loader<T>
    {
        public Loader() : base("ELEMENTS/loader-black-24")
        {
            VisualElement.AddToClassList("elements-loader");
            Disposables.Add(ElementPortal.RegisterUpdate(TickAnimation));
        }

        private void TickAnimation(float deltaTime)
        {
            var angle = VisualElement.style.rotate.value.angle.value + 360f * deltaTime;
            if (angle >= 360f) angle -= 360f;
            VisualElement.style.rotate = new Rotate(angle);
        }
    }

    public class Loader : Loader<Loader>
    {
    }
}

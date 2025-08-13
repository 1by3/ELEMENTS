using UnityEngine.UIElements;

namespace ELEMENTS.Scripts.Elements
{
    public class Loader<T> : Image<T> where T : Loader<T>
    {
        public Loader() : base("ELEMENTS/loader-black-24")
        {
            VisualElement.AddToClassList("elements-loader");
            OnUpdate += TickAnimation;
        }

        private void TickAnimation(object sender, float deltaTime)
        {
            var degreesPerSecond = 360f;
            var angle = VisualElement.style.rotate.value.angle.value + degreesPerSecond * deltaTime;
            if (angle >= 360f) angle -= 360f;
            VisualElement.style.rotate = new Rotate(angle);
        }
    }

    public class Loader : Loader<Loader>
    {
    }
}
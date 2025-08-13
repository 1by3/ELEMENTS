using R3;

namespace ELEMENTS.Elements
{
    public class ProgressBar<T> : BaseElement<T> where T : ProgressBar<T>
    {
        public ProgressBar(float progress)
        {
            VisualElement = new UnityEngine.UIElements.ProgressBar
            {
                value = progress
            };
        }

        public ProgressBar() : this(0f)
        {
        }

        public ProgressBar(Observable<float> progress) : this()
        {
            BindProgress(progress);
        }

        public T Progress(float progress)
        {
            ((UnityEngine.UIElements.ProgressBar)VisualElement).value = progress;
            return (T)this;
        }

        public float GetProgress()
        {
            return ((UnityEngine.UIElements.ProgressBar)VisualElement).value;
        }

        public T BindProgress(Observable<float> progress)
        {
            Disposables.Add(progress.Subscribe(nv => Progress(nv)));
            return (T)this;
        }

    }

    public class ProgressBar : ProgressBar<ProgressBar>
    {
        public ProgressBar(float progress) : base(progress) { }
        public ProgressBar() : base(0) { }
        public ProgressBar(Observable<float> progress) : base(progress) { }
    }
}
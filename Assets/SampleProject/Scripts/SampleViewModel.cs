using ELEMENTS.MVVM;
using R3;

namespace SampleProject.Scripts
{
    public class SampleViewModel : ViewModel
    {
        public readonly ReactiveProperty<int> Count = new(0);
        public readonly ReactiveProperty<bool> AlertOpen = new(false);

        public void OpenAlert()
        {
            AlertOpen.Value = true;
        }

        public void CloseAlert()
        {
            AlertOpen.Value = false;
        }

        public void Increment()
        {
            Count.Value++;
        }

        public void Decrement()
        {
            Count.Value--;
        }

        public void Reset()
        {
            Count.Value = 0;
        }

        public void Double()
        {
            Count.Value *= 2;
        }
    }
}
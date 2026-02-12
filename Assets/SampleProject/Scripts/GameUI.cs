using ELEMENTS.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace SampleProject.Scripts
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private void OnEnable()
        {
            uiDocument.AddStyleSheet("ELEMENTS/DefaultStyles");
            uiDocument.RenderElement(new SampleComponent());
        }
    }
}
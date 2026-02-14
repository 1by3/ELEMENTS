using System;
using ELEMENTS.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace SampleProject.Scripts
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;

        private IDisposable ui;

        private void OnEnable()
        {
            uiDocument.AddStyleSheet("ELEMENTS/DefaultStyles");
            uiDocument.AddStyleSheet("ELEMENTS/ExtendedStyles");
            ui = uiDocument.RenderElement(new SampleComponent());
        }

        private void OnDisable()
        {
            ui?.Dispose();
            ui = null;
        }
    }
}
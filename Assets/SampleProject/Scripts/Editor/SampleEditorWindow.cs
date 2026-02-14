using System;
using ELEMENTS.Editor;
using UnityEditor;
using UnityEngine;

namespace SampleProject.Scripts.Editor
{
    public class SampleEditorWindow : EditorWindow
    {
        [MenuItem("ELEMENTS/Sample Window")]
        public static void ShowPreferences() => GetWindow<SampleEditorWindow>().Show();

        private void OnEnable()
        {
            titleContent = new GUIContent("ELEMENTS");
            this.RenderElement(new SampleEditorComponent());
        }
    }
}

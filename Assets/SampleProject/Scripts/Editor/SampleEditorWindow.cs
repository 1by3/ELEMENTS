using ELEMENTS.Scripts.Editor;
using UnityEditor;
using UnityEngine;

namespace SampleProject.Scripts.Editor
{
    public class SampleEditorWindow : ElementEditorWindow<SampleViewModel, SampleEditorView>
    {
        [MenuItem("ELEMENTS/Sample Window")]
        public static void ShowPreferences()
        {
            var window = GetWindow<SampleEditorWindow>();
            window.titleContent = new GUIContent("ELEMENTS");
            window.Show();
        }
    }
}
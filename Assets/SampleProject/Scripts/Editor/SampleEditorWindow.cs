using ELEMENTS.Editor;
using UnityEditor;
using UnityEngine;

namespace SampleProject.Scripts.Editor
{
    public class SampleEditorWindow : ElementEditorWindow<SampleEditorComponent>
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

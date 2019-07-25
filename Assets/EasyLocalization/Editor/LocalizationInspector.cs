using UnityEditor;
using UnityEngine;

namespace EasyLocalization.Editor
{
    [CustomEditor(typeof(Localization), true)]
    public class LocalizationInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Reload"))
            {
                (target as Localization).Reload();
            }
        }
    }
}

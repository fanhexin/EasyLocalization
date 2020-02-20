using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EasyLocalization.Editor
{
    [CustomEditor(typeof(Localization))]
    public class LocalizationInspector : UnityEditor.Editor
    {
        private Localization _target;
        private SerializedProperty _dataProvider;

        private void OnEnable()
        {
            _target = target as Localization;
            _dataProvider = serializedObject.FindProperty("_dataProvider");
        }

        public override void OnInspectorGUI()
        {
            using (var cc = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(_dataProvider);

                if (_target.dataProvider != null)
                {
                    string[] languages = _target.dataProvider.Languages.ToArray();
                    int index = Array.FindIndex(languages, v => v == _target.language);
                    if (index < 0)
                    {
                        index = 0;
                    }
                    index = EditorGUILayout.Popup("Default Language", index, languages);
                    _target.language = languages[index];
                }
                
                if (cc.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            if (GUILayout.Button("Reload"))
            {
                foreach (var localizationText in FindObjectsOfType<LocalizationText>())
                {
                    localizationText.GetType()
                        .GetMethod("UpdateText", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(localizationText, null);
                }
            }
        }
    }
}
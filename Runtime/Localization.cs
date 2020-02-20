using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyLocalization
{
    public class Localization : ScriptableObject
    {
        static Localization _instance;
        public static Localization instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<Localization>(nameof(Localization));
                }
#if UNITY_EDITOR
                if (_instance == null)
                {
                    _instance = CreateInstance<Localization>();
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }
                    AssetDatabase.CreateAsset(_instance, $"Assets/Resources/{nameof(Localization)}.asset");
                    AssetDatabase.Refresh();
                }
#endif
                return _instance;
            }
        }

        public event Action changed;

        [SerializeField]
        private DataProvider _dataProvider;
        public DataProvider dataProvider => _dataProvider;

        [SerializeField]
        private string _defaultLanguage;

        private string _language;

        public string language
        {
            get => _language;
            set
            {
                if (_language == value)
                {
                    return;
                }
                
                _language = value;
                PlayerPrefs.SetString(nameof(Localization), value);
                changed?.Invoke();
            }
        }

        public string this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return string.Empty;
                }

                if (_dataProvider == null)
                {
                    return key;
                }

                return _dataProvider.GetValue(language, key);
            }
        }

        private void OnEnable()
        {
            _language = PlayerPrefs.GetString(nameof(Localization), _defaultLanguage);
        }

#if UNITY_EDITOR
        public IEnumerable<string> keys => _dataProvider.Keys;
#endif
    }
}

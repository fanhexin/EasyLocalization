using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace EasyLocalization
{
    public abstract class Localization : ScriptableObject
    {
        static Localization _instance;
        public static Localization instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {
                    string[] ret = AssetDatabase.FindAssets($"t: {nameof(Localization)}");
                    if (ret == null || ret.Length == 0)
                    {
                        throw new Exception($"{nameof(Localization)}'s implementation not found!");
                    }

                    _instance = AssetDatabase.LoadAssetAtPath<Localization>(AssetDatabase.GUIDToAssetPath(ret[0]));
                }
#endif
                return _instance;
            }
        }

        public event Action ZoneChangeEv;
    
        public string this[string key]
        {
            get
            {
                if (string.IsNullOrEmpty(key))
                {
                    return string.Empty;
                }
                
                _dic.TryGetValue(key, out string value);
                return value;
            }
        }

        [NonSerialized]
        IReadOnlyDictionary<string, string> _dic;
        
        protected Localization()
        {
            if (_instance != null)
            {
                throw new Exception("Localization need to be singleton!");
            }
            _instance = this;
        }

        protected virtual void OnEnable()
        {
            _dic = LoadDic();
        }
        
#if UNITY_EDITOR
        public IEnumerable<string> keys => _dic.Keys;
        public void Reload()
        {
            UpdateDic(LoadDic());
        }
        
        void Reset()
        {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.Exists(x => x is Localization))
            {
                return;
            }
            preloadedAssets.Add(this);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }
#endif

        protected abstract IReadOnlyDictionary<string, string> LoadDic();

        public void UpdateDic(IReadOnlyDictionary<string, string> dic)
        {
            if (_dic == dic)
            {
                return;
            }
            
            _dic = dic;
            ZoneChangeEv?.Invoke();
        }
    }
}

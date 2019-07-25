using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyLocalization
{
    public abstract class Localization : ScriptableObject
    {
        public static Localization instance { get; private set; }

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
            if (instance != null)
            {
                throw new Exception($"{name} need to be singleton!");
            }
            instance = this;
        }

        protected virtual void OnEnable()
        {
            _dic = LoadDic();
        }

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
        
#if UNITY_EDITOR
        public IEnumerable<string> keys => _dic.Keys;
        public void Reload()
        {
            UpdateDic(LoadDic());
        }
#endif
    }
}

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EasyLocalization
{
    [AddComponentMenu("UI/LocalizationText", 11)]
    public class LocalizationText : Text
    {
        [SerializeField] string _localizationKey;
        [SerializeField] Localization _localization;

        public string localizationKey
        {
            set
            {
                _localizationKey = value;
                UpdateText();
            }
        }

        object[] _formatArgs;

        #if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            if (_localization != null)
            {
                return;
            }

            string[] ret = AssetDatabase.FindAssets($"t: {nameof(Localization)}");
            if (ret == null || ret.Length == 0)
            {
                Debug.LogError($"{nameof(Localization)}'s implementation not found!");
                return;
            }

            _localization = AssetDatabase.LoadAssetAtPath<Localization>(AssetDatabase.GUIDToAssetPath(ret[0]));
        }
        #endif

        protected override void Start()
        {
            base.Start();
            UpdateText();
            _localization.ZoneChangeEv += UpdateText;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _localization.ZoneChangeEv -= UpdateText;
        }

        void UpdateText()
        {
            string value = _localization[_localizationKey];
            if (!string.IsNullOrEmpty(value))
            {
                text = _formatArgs == null ? value : string.Format(value, _formatArgs);
            }
        }

        public void Format(params object[] args)
        {
            FormatByKey(_localizationKey, args);
        }

        public void FormatByKey(string localizationKey, params object[] args)
        {
            string value = _localization[localizationKey];
            text = string.Format(string.IsNullOrEmpty(value)?text:value, args);
            _formatArgs = args;
        }
    }
}

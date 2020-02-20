using UnityEngine;
using UnityEngine.UI;

namespace EasyLocalization
{
    [AddComponentMenu("UI/LocalizationText", 11)]
    public class LocalizationText : Text
    {
        [SerializeField] string _localizationKey;

        public string localizationKey
        {
            set
            {
                _localizationKey = value;
                UpdateText();
            }
        }

        object[] _formatArgs;

        protected override void Start()
        {
            base.Start();
            UpdateText();
            Localization.instance.changed += UpdateText;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Localization.instance.changed -= UpdateText;
        }

        void UpdateText()
        {
            string value = Localization.instance[_localizationKey];
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
            string value = Localization.instance[localizationKey];
            text = string.Format(string.IsNullOrEmpty(value)?text:value, args);
            _formatArgs = args;
        }
    }
}

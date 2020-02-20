using EasyLocalization;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    Localization _localization;
    
    // Start is called before the first frame update

    [ContextMenu("changdic")]
    void ChangeDic()
    {
        _localization.language = SystemLanguage.Chinese.ToString();
    }
}

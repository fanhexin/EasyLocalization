using System.Collections.Generic;
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
        _localization.UpdateDic(new Dictionary<string, string>
        {
            {"A", "A"},
            {"B", "B"}
        });
    }
}

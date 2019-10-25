using System.Collections.Generic;
using EasyLocalization;
using UnityEngine;

[CreateAssetMenu(menuName = "Localization", fileName = "TestLocalization")]
public class TestLocalization : Localization
{
    protected override IReadOnlyDictionary<string, string> LoadDic()
    {
        return new Dictionary<string, string>
        {
            {"MAIN_TITLE", "Main Title"},
            {"CLOSE_BTN", "Close"}
        };
    }
}
using System;
using System.Collections.Generic;
using EasyLocalization;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TestDataProvider), menuName = nameof(TestDataProvider))]
public class TestDataProvider : DataProvider
{
    public override IEnumerable<string> Keys => _dic[SystemLanguage.English.ToString()].Keys;
    

    public override IEnumerable<string> Languages
    {
        get
        {
            yield return SystemLanguage.English.ToString();
            yield return SystemLanguage.Chinese.ToString();
        }
    }
    public override string GetValue(string language, string key)
    {
        return _dic[language][key];
    }

    [NonSerialized]
    private Dictionary<string, Dictionary<string, string>> _dic;

    private void OnEnable()
    {
        _dic = new Dictionary<string, Dictionary<string, string>>
        {
            ["English"] = new Dictionary<string, string> {{"TITLE_1", "title_1"}, {"TITLE_2", "title_2"}},
            ["Chinese"] = new Dictionary<string, string> {{"TITLE_1", "标题1"}, {"TITLE_2", "标题2"}}
        };
    }
}

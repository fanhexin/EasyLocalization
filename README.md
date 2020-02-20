# EasyLocalization

## 安装

可通过修改`manifest.json`，在其中加入如下条目安装：

```json
{
  "com.github.fanhexin.easylocalization": "https://github.com/fanhexin/EasyLocalization.git#upm"
}
```

## 创建`Localization`子类

`EasyLocalization`不对具体的多语言数据文件格式和加载方式做限制，使用者只需创建继承自抽象类`DataProvider`的实体子类，实现抽象方法。`DataProvider`本身继承自`ScriptableObject`，实现`DataProvider`的子类需要在工程内创建一份`ScriptableObject`资源文件，并拖入`Localization`资源文件的引用中。具体示例如下：

```csharp
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
```

## UGUI界面中添加`LocalizationText`

在场景中通过`UI > LocalizationText`添加带有多语言功能的`Text`控件，其带有自定义的`Inspector`，可直接通过`LocalizationKey`选择所关联的字典`Key`。 如下图所示：

![LocalizationTextInspector](Images/LocalizationTextInspector.png)

## 代码中查询`Key`值

创建`Localization`的子类后，运行时可通过`Localization.instance`访问实例。`Localization`实现了`Indexer`，可通过[]语法直接查询。

```csharp
string value = Localization.instance["key值"];
```

## 切换语言时更新文本

在切换语言时，需要设置`Localization`的`language`属性，对内部字典进行更新。更新结果会通过`LocalizationText`进行实时展示。

```csharp
Localization.instance.language = "Chinese";
```

## Localization编辑器下的Reload功能

如在编辑器启动的状态下，多语言配置文件发生了更新，这时需要进行重新加载，以便`LocalizationText Inspector`上能显示新的Key或Value值。
可通过点击`Localization`资源文件`Inspector`上的`Reload`按钮实现。如下图：

![Reload](Images/Reload.png)

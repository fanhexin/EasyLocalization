using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;
using TextEditor = UnityEditor.UI.TextEditor;

namespace EasyLocalization.Editor
{
    public class LocalizationKeySelectWindow : PopupWindowContent
    {
        readonly float _width;
        readonly float _height;
        readonly SearchField _searchField = new SearchField();
        private readonly ListView _listView;
        public event Action<string> selectKeyEv;

        public LocalizationKeySelectWindow(float width, float height)
        {
            _width = width;
            _height = height;
            _listView = new ListView(new TreeViewState(), Localization.instance.keys);
        }

        void OnSelectionChanged(string key)
        {
            selectKeyEv?.Invoke(key);
            editorWindow.Close();
        }

        public override void OnOpen()
        {
            base.OnOpen();
            _listView.OnSelectionChanged += OnSelectionChanged;
        }

        public override void OnClose()
        {
            base.OnClose();
            _listView.OnSelectionChanged -= OnSelectionChanged;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(_width, _height);
        }

        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            _listView.searchString = _searchField.OnToolbarGUI(_listView.searchString);
            GUILayout.Space(5); 
            if (GUILayout.Button("Clear", EditorStyles.toolbarButton) && selectKeyEv != null)
            {
                selectKeyEv(string.Empty);
                editorWindow.Close();
            }
            
            EditorGUILayout.EndHorizontal();

            var listViewRect = GUILayoutUtility.GetRect(rect.width, rect.height - EditorStyles.foldout.fixedHeight);
            _listView.OnGUI(listViewRect);
        }
    }
    
    [CustomEditor(typeof(LocalizationText), true)]
    [CanEditMultipleObjects]
    public class LocalizationTextEditor : TextEditor
    {
        SerializedProperty _localizationKeyProp;
        LocalizationText _target;
        
        [MenuItem("GameObject/UI/LocalizationText", false, 2000)]
        public static void AddText(MenuCommand menuCommand)
        {
            var go = new GameObject("Text");
            var rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(160, 30);
            var lt = go.AddComponent<LocalizationText>();
            LocalizationTextDefaultSetting(lt);

            var parent = menuCommand.context as GameObject;
            if (parent == null || parent.GetComponentInParent<Canvas>() == null)
            {
                return;
            }
            string uniqueName = GameObjectUtility.GetUniqueNameForSibling(parent.transform, go.name);
            go.name = uniqueName;
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Undo.SetTransformParent(go.transform, parent.transform, "Parent " + go.name);
            GameObjectUtility.SetParentAndAlign(go, parent);
            Selection.activeGameObject = go;
        }

        [MenuItem("CONTEXT/Text/ToLocalizationText")]
        public static void Text2LocalizationText(MenuCommand menuCommand)
        {
            var parent = (menuCommand.context as Text).gameObject;
            if (parent == null)
            {
                return;
            }

            var texts = parent.GetComponentsInChildren<Text>();
            foreach (var t in texts)
            {
                GameObject go = t.gameObject;
                Font font = t.font;
                int fontSize = t.fontSize;
                var fontStyle = t.fontStyle;
                var lineSpacing = t.lineSpacing;
                var richText = t.supportRichText;
                var alignment = t.alignment;
                var alignByGeometry = t.alignByGeometry;
                var hOverflow = t.horizontalOverflow;
                var vOverflow = t.verticalOverflow;
                var bestFit = t.resizeTextForBestFit;
                var color = t.color;
//                var mat = t.material;
                var raycastTarget = t.raycastTarget;
                
                Undo.DestroyObjectImmediate(t);
                var lt = Undo.AddComponent<LocalizationText>(go);
//                LocalizationTextDefaultSetting(lt);
                lt.font = font;
                lt.fontSize = fontSize;
                lt.fontStyle = fontStyle;
                lt.lineSpacing = lineSpacing;
                lt.supportRichText = richText;
                lt.alignment = alignment;
                lt.alignByGeometry = alignByGeometry;
                lt.horizontalOverflow = hOverflow;
                lt.verticalOverflow = vOverflow;
                lt.resizeTextForBestFit = bestFit;
                lt.color = color;
//                if (mat) lt.material = mat;
                lt.raycastTarget = raycastTarget;
                lt.text = t.text;
            }
        }

        [MenuItem("CONTEXT/Text/ToLocalizationText", true)]
        public static bool Text2LocalizationTextValidate(MenuCommand menuCommand)
        {
            return menuCommand.context is Text && !(menuCommand.context is LocalizationText);
        }

        static void LocalizationTextDefaultSetting(LocalizationText lt)
        {
            lt.raycastTarget = false;
            lt.supportRichText = false;
            lt.alignment = TextAnchor.MiddleCenter;
            lt.horizontalOverflow = HorizontalWrapMode.Overflow;
            lt.verticalOverflow = VerticalWrapMode.Overflow;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _localizationKeyProp = serializedObject.FindProperty("_localizationKey");
            _target = target as LocalizationText;
        }

        Rect _tmpRect;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("LocalizationKey");
            if (GUILayout.Button(_localizationKeyProp.stringValue, EditorStyles.popup))
            {
                var localizationKeySelectWindow = new LocalizationKeySelectWindow(_tmpRect.width, 200);
                localizationKeySelectWindow.selectKeyEv += OnLocalizationKeySelectWindowOnSelectKeyEv;
                PopupWindow.Show(_tmpRect, localizationKeySelectWindow);
            }
            if (Event.current.type == EventType.Repaint) _tmpRect = GUILayoutUtility.GetLastRect();
            EditorGUILayout.EndHorizontal();
            
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }

        // TODO 添加对批量设置的支持
        void OnLocalizationKeySelectWindowOnSelectKeyEv(string v)
        {
            _target.text = Localization.instance[v];
            serializedObject.Update();
            _localizationKeyProp.stringValue = v;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
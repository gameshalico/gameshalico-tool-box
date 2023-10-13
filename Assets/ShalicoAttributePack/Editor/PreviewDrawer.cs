using UnityEditor;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 通常のプロパティを描画
            EditorGUI.PropertyField(position, property, label);

            // PreviewAttributeの取得
            var previewAttribute = (PreviewAttribute)attribute;

            // プレビュー用テクスチャの取得
            var texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);

            // 右寄せにして画像を表示
            var style = new GUIStyle(GUI.skin.label)
            {
                margin = new RectOffset(10, 10, 10, 10)
            };

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(
                new GUIContent(texture), style,
                GUILayout.Height(previewAttribute.Height));
            GUILayout.EndHorizontal();
        }
    }
}
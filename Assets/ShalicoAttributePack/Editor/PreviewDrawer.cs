using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewDrawer : PropertyDrawer
    {
        private static readonly float Margin = EditorGUIUtility.standardVerticalSpacing;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var previewAttribute = (PreviewAttribute)attribute;
            var image = new Image
            {
                style =
                {
                    height = previewAttribute.Height,
                    marginTop = Margin,
                    marginBottom = Margin,
                    marginLeft = Margin,
                    marginRight = Margin,
                    alignSelf = Align.FlexEnd
                }
            };

            var propertyField = new PropertyField(property);

            container.Add(propertyField);
            container.Add(image);

            propertyField.RegisterCallback<ChangeEvent<Object>>(evt => { SetImage(evt.newValue, image); });
            SetImage(property.objectReferenceValue, image);

            return container;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
            var previewAttribute = (PreviewAttribute)attribute;
            var texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
            if (texture == null) return;

            var width = previewAttribute.Height * texture.width / texture.height;
            var imagePosition = new Rect(position.x + position.width - width, position.y, width,
                previewAttribute.Height);
            GUI.DrawTexture(imagePosition, texture);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var previewAttribute = (PreviewAttribute)attribute;
            return EditorGUI.GetPropertyHeight(property, label) + previewAttribute.Height + Margin * 2;
        }

        private void SetImage(Object value, Image image)
        {
            var previewAttribute = (PreviewAttribute)attribute;
            var texture = AssetPreview.GetAssetPreview(value);
            image.image = texture;
            image.MarkDirtyRepaint();

            if (texture == null) return;

            var width = previewAttribute.Height * texture.width / texture.height;
            image.style.width = width;
        }
    }
}
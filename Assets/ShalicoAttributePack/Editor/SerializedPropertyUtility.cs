using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace ShalicoAttributePack.Editor
{
    public static class SerializedPropertyUtility
    {
        /// <summary>
        ///     SerializedProperty.propertyPathを取得しやすいように整形し分割する。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string[] SplitPathParts(string path)
        {
            var pathParts = path.Split('.');
            var resultList = new List<string>();

            for (var i = 0; i < pathParts.Length; i++)
            {
                var part = pathParts[i];
                if (part.Contains("["))
                {
                    var arrayPart = pathParts[i - 2];
                    var indexPart = part.Substring(part.IndexOf('[')).Replace("[", "").Replace("]", "");
                    var index = int.Parse(indexPart);

                    var arrayString = arrayPart + "[" + index + "]";

                    resultList.RemoveRange(resultList.Count - 2, 2);
                    resultList.Add(arrayString);
                }
                else
                {
                    resultList.Add(part);
                }
            }

            return resultList.ToArray();
        }

        private static object AnalyzePathParts(string[] pathParts, object currentObject)
        {
            foreach (var part in pathParts)
            {
                var previousObject = currentObject;
                // 配列やリストの要素にアクセスする場合の処理
                if (part.Contains("["))
                {
                    var arrayPart = part.Substring(0, part.IndexOf('['));
                    var indexPart = part.Substring(part.IndexOf('[')).Replace("[", "").Replace("]", "");
                    var index = int.Parse(indexPart);

                    currentObject = GetValueWithIndex(currentObject, arrayPart, index);
                }
                else
                {
                    currentObject = GetValue(currentObject, part);
                }

                if (currentObject == null) return null;
            }

            return currentObject;
        }

        public static object GetNestedPropertyValue(this SerializedProperty property)
        {
            var pathParts = SplitPathParts(property.propertyPath);
            object currentObject = property.serializedObject.targetObject;
            if (pathParts.Length == 1) return currentObject;

            return AnalyzePathParts(pathParts, currentObject);
        }

        public static object GetParentObject(this SerializedProperty property)
        {
            var pathParts = SplitPathParts(property.propertyPath);
            object parentObject = property.serializedObject.targetObject;

            if (pathParts.Length == 1) return parentObject;

            var parentPathParts = new string[pathParts.Length - 1];
            for (var i = 0; i < parentPathParts.Length; i++)
                parentPathParts[i] = pathParts[i];

            return AnalyzePathParts(parentPathParts, parentObject);
        }

        private static object GetValue(object source, string name)
        {
            if (source == null) return null;
            var type = source.GetType();

            var field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field == null) return null;

            var value = field.GetValue(source);

            return value;
        }

        private static object GetValueWithIndex(object source, string name, int index)
        {
            var value = GetValue(source, name);

            if (value == null) return null;

            if (index >= 0)
            {
                var enumerable = value as IEnumerable;
                if (enumerable == null) return null;

                var enumerator = enumerable.GetEnumerator();
                for (var i = 0; i <= index; i++)
                    if (!enumerator.MoveNext())
                        return null;

                value = enumerator.Current;
            }

            return value;
        }
    }
}
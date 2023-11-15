using System;
using System.Collections;
using System.Collections.Generic;
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
        public static string[] SplitPathParts(string path)
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

        public static object ParsePathPart(string part, object currentObject)
        {
            var previousObject = currentObject;
            // 配列やリストの要素にアクセスする場合の処理
            if (part.Contains("["))
            {
                var arrayPart = part.Substring(0, part.IndexOf('['));
                var indexPart = part.Substring(part.IndexOf('[')).Replace("[", "").Replace("]", "");
                var index = int.Parse(indexPart);

                currentObject = GetFieldValueWithIndex(currentObject, arrayPart, index);
            }
            else
            {
                currentObject = GetFieldValue(currentObject, part);
            }


            return currentObject;
        }

        public static Type GetFieldElementType(string part, object currentObject)
        {
            if (part.Contains("["))
            {
                var arrayPart = part.Substring(0, part.IndexOf('['));

                if (ReflectionUtility.TryFindFieldInfo(currentObject, arrayPart, out var fieldInfo))
                {
                    if (fieldInfo.FieldType.IsArray)
                        return fieldInfo.FieldType.GetElementType();
                    if (fieldInfo.FieldType.IsGenericType)
                        return fieldInfo.FieldType.GetGenericArguments()[0];
                }
            }

            if (ReflectionUtility.TryFindFieldInfo(currentObject, part, out var info))
                return info.FieldType;

            return null;
        }

        public static object AnalyzePathParts(string[] pathParts, object currentObject)
        {
            foreach (var part in pathParts) currentObject = ParsePathPart(part, currentObject);

            return currentObject;
        }

        public static object TracePathParts(this SerializedProperty property, string[] pathParts)
        {
            var currentObject = property.serializedObject.targetObject;

            return AnalyzePathParts(pathParts, currentObject);
        }

        /// <summary>
        ///     SerializedProperty.propertyPathの値を取得する。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object TracePropertyValue(this SerializedProperty property)
        {
            var pathParts = SplitPathParts(property.propertyPath);

            return TracePathParts(property, pathParts);
        }

        /// <summary>
        ///     SerializedProperty.propertyPathの親の値を取得する。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object TraceParentValue(this SerializedProperty property)
        {
            var pathParts = SplitPathParts(property.propertyPath);
            object parentObject = property.serializedObject.targetObject;

            if (pathParts.Length == 1) return parentObject;

            var parentPathParts = new string[pathParts.Length - 1];
            for (var i = 0; i < parentPathParts.Length; i++)
                parentPathParts[i] = pathParts[i];

            return AnalyzePathParts(parentPathParts, parentObject);
        }

        private static object GetFieldValue(object source, string name)
        {
            if (source == null) throw new ArgumentNullException(nameof(source) + " is null.");

            if (ReflectionUtility.TryFindFieldValue(source, name, out var value))
                return value;

            throw new ArgumentException(name + " is not a field of " + source.GetType());
        }

        private static object GetFieldValueWithIndex(object source, string name, int index)
        {
            var value = GetFieldValue(source, name);

            if (value == null) return null;

            var enumerable = value as IEnumerable;
            if (enumerable == null) return null;

            var enumerator = enumerable.GetEnumerator();
            for (var i = 0; i <= index; i++)
                if (!enumerator.MoveNext())
                    return null;

            value = enumerator.Current;

            return value;
        }
    }
}
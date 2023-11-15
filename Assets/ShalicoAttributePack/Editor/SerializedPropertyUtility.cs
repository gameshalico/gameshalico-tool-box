using System;
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

        private static object GetNext(string part, object currentObject)
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

        private static FieldInfo GetNextFieldInfo(string part, object currentObject)
        {
            if (part.Contains("["))
                part = part.Substring(0, part.IndexOf('['));

            if (ReflectionUtility.TryFindFieldInfo(currentObject, part, out var info))
                return info;
            return null;
        }

        private static object AnalyzePathParts(string[] pathParts, object currentObject)
        {
            foreach (var part in pathParts) currentObject = GetNext(part, currentObject);

            return currentObject;
        }


        /// <summary>
        ///     SerializedProperty.propertyPathの値を取得する。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object TracePropertyValue(this SerializedProperty property)
        {
            var pathParts = SplitPathParts(property.propertyPath);

            return AnalyzePathParts(pathParts, property.serializedObject.targetObject);
        }

        public static (object obj, FieldInfo info) TracePropertyValueWithFieldInfo(this SerializedProperty property)
        {
            var pathParts = SplitPathParts(property.propertyPath);
            object root = property.serializedObject.targetObject;

            var lastPart = pathParts[^1];
            Array.Resize(ref pathParts, pathParts.Length - 1);

            var parent = AnalyzePathParts(pathParts, root);
            var obj = GetNext(lastPart, parent);
            var info = GetNextFieldInfo(lastPart, parent);

            return (obj, info);
        }

        /// <summary>
        ///     SerializedProperty.propertyPathの親の値を取得する。
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object TraceParentValue(this SerializedProperty property)
        {
            var pathParts = SplitPathParts(property.propertyPath);
            object root = property.serializedObject.targetObject;

            if (pathParts.Length == 1) return root;

            Array.Resize(ref pathParts, pathParts.Length - 1);

            return AnalyzePathParts(pathParts, root);
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
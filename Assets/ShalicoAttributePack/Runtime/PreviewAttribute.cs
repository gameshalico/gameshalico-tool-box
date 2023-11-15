using System;
using UnityEngine;

namespace ShalicoAttributePack
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PreviewAttribute : PropertyAttribute
    {
        public float Height = 50;
    }
}
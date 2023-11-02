using System;
using UnityEngine;

namespace ShalicoPalette
{
    [Serializable]
    public struct IndexedColor
    {
        public int colorIndex;

        public IndexedColor(int colorIndex)
        {
            this.colorIndex = colorIndex;
        }

        public Color32 Resolve()
        {
            return ColorResolver.Resolve(this);
        }

        public static implicit operator IndexedColor(int colorIndex)
        {
            return new IndexedColor(colorIndex);
        }

        public static implicit operator Color32(IndexedColor indexedColor)
        {
            return indexedColor.Resolve();
        }

        public static implicit operator Color(IndexedColor indexedColor)
        {
            return indexedColor.Resolve();
        }
    }
}
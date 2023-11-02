using ShalicoAttributePack;
using ShalicoPalette;
using UnityEngine;

namespace Samples.Palette
{
    public class ColorSample : MonoBehaviour
    {
        [Readonly] public Color color;
        public HueTone hueTone;

        [SerializeField] private IndexedColor indexedColor;

        private void OnValidate()
        {
            color = indexedColor;
        }
    }
}
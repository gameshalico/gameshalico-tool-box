using ShalicoAttributePack;
using ShalicoPalette;
using UnityEngine;

namespace Samples.Palette
{
    public class ColorSample : MonoBehaviour
    {
        public bool complementary;

        [Readonly] public Color color;
        public HueTone hueTone;

        private void OnValidate()
        {
            color = hueTone.GetColor();
            if (complementary) color = color.GetComplementary();
        }
    }
}
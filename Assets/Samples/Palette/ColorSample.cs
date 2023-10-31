using ShalicoAttributePack;
using ShalicoPalette;
using UnityEngine;

namespace Samples.Palette
{
    public class ColorSample : MonoBehaviour
    {
        [Readonly] public Color color;
        public HueTone hueTone;
        public bool complementary;

        private void OnValidate()
        {
            color = hueTone.GetColor();
            if (complementary) color = color.GetComplementary();
        }
    }
}
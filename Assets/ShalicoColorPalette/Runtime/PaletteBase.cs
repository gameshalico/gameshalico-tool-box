using UnityEngine;

namespace ShalicoColorPalette
{
    public abstract class PaletteBase : ScriptableObject, IPalette
    {
        public abstract bool TryGetColorData(int index, out IColorData colorData);
        public abstract bool TryFindColorDataByName(string colorName, out IColorData colorData);
        public abstract IColorData[] GetAllColorData();
    }
}
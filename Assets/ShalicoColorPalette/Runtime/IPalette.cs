namespace ShalicoColorPalette
{
    public interface IPalette
    {
        public bool TryGetColorData(int index, out IColorData colorData);
        public bool TryFindColorDataByName(string colorName, out IColorData colorData);
        public IColorData[] GetAllColorData();
    }
}
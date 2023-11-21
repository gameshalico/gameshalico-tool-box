namespace ShalicoColorPalette
{
    public static class HueSymbolExtensions
    {
        public static HueSymbol Shift(this HueSymbol self, int shift)
        {
            return (HueSymbol)(((int)self - 1 + shift) % 24 + 1);
        }
    }
}
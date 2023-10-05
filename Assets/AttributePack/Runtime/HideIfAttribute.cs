namespace AttributePack
{
    public class HideIfAttribute : BoolNameAttribute
    {
        public HideIfAttribute(string propertyName) : base(propertyName)
        {
        }
    }
}
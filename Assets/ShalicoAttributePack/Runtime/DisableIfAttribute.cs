namespace AttributePack
{
    public class DisableIfAttribute : BoolNameAttribute
    {
        public DisableIfAttribute(string propertyName) : base(propertyName)
        {
        }
    }
}
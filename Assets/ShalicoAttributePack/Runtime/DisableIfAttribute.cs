namespace ShalicoAttributePack
{
    public class DisableIfAttribute : BoolNameAttribute
    {
        public DisableIfAttribute(string propertyName) : base(propertyName)
        {
        }
    }
}
namespace ShalicoAttributePack.Runtime
{
    public class DisableIfAttribute : BoolNameAttribute
    {
        public DisableIfAttribute(string propertyName) : base(propertyName)
        {
        }
    }
}
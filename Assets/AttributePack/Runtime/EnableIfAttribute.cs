namespace AttributePack
{
    public class EnableIfAttribute : BoolNameAttribute
    {
        public EnableIfAttribute(string propertyName) : base(propertyName)
        {
        }
    }
}
namespace ShalicoAttributePack
{
    public class EnableIfAttribute : ConditionAttribute
    {
        public EnableIfAttribute(string name, object value) : base(name, value)
        {
        }
    }
}
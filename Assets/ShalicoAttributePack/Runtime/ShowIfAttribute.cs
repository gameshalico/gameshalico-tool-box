namespace ShalicoAttributePack
{
    public class ShowIfAttribute : ConditionAttribute
    {
        public ShowIfAttribute(string propertyConditionName, object value) : base(propertyConditionName, value)
        {
        }
    }
}
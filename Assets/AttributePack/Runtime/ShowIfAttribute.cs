﻿namespace AttributePack
{
    public class ShowIfAttribute : BoolNameAttribute
    {
        public ShowIfAttribute(string propertyName) : base(propertyName)
        {
        }
    }
}
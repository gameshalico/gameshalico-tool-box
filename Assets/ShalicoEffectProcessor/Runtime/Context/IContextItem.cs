using System;

namespace ShalicoEffectProcessor.Context
{
    public interface IContextItem : ICloneable
    {
        void OnRelease()
        {
        }
    }
}
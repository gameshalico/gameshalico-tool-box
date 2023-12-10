using System;

namespace ShalicoToolBox
{
    [Serializable]
    public struct WeightedElement<T>
    {
        public T element;
        public float weight;

        public WeightedElement(T element, float weight)
        {
            this.element = element;
            this.weight = weight;
        }
    }
}
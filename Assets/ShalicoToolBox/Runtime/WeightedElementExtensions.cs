using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace ShalicoToolBox
{
    public static class WeightedElementExtensions
    {
        public static float GetTotalWeight<T>(this IEnumerable<WeightedElement<T>> elements)
        {
            var totalWeight = 0f;
            foreach (var element in elements) totalWeight += element.weight;

            return totalWeight;
        }

        public static WeightedElement<T> TakeByWeight<T>(this IEnumerable<WeightedElement<T>> elements, float weight)
        {
            foreach (var element in elements)
            {
                if (weight < element.weight) return element;
                weight -= element.weight;
            }

            throw new ArgumentOutOfRangeException(nameof(weight));
        }

        public static WeightedElement<T> TakeRandomByWeight<T>(this WeightedElement<T>[] elements)
        {
            var totalWeight = elements.GetTotalWeight();
            var randomValue = Random.Range(0, totalWeight);
            return elements.TakeByWeight(randomValue);
        }

        public static WeightedElement<T> TakeRandomByWeight<T>(this IReadOnlyList<WeightedElement<T>> elements)
        {
            var totalWeight = elements.GetTotalWeight();
            var randomValue = Random.Range(0, totalWeight);
            return elements.TakeByWeight(randomValue);
        }

        public static T[] TakeRandomByWeight<T>(this WeightedElement<T>[] elements, int count)
        {
            var totalWeight = elements.GetTotalWeight();

            var result = new T[count];
            for (var i = 0; i < count; i++)
            {
                var randomValue = Random.Range(0, totalWeight);
                result[i] = elements.TakeByWeight(randomValue).element;
            }

            return result;
        }

        public static T[] TakeRandomByWeight<T>(this IReadOnlyList<WeightedElement<T>> elements, int count)
        {
            var totalWeight = elements.GetTotalWeight();

            var result = new T[count];
            for (var i = 0; i < count; i++)
            {
                var randomValue = Random.Range(0, totalWeight);
                result[i] = elements.TakeByWeight(randomValue).element;
            }

            return result;
        }

        public static T[] TakeRandomDistinctByWeight<T>(this IEnumerable<WeightedElement<T>> elements, int count)
        {
            var elementList = elements.ToList();
            if (count > elementList.Count()) throw new ArgumentOutOfRangeException(nameof(count));

            var result = new T[count];

            for (var i = 0; i < count; i++)
            {
                var weightedElement = elementList.TakeRandomByWeight();
                result[i] = weightedElement.element;
                elementList.Remove(weightedElement);
            }

            return result;
        }

        public static WeightedElement<T> TakeWeightedElementByRate<T>(WeightedElement<T>[] elements,
            float weightRate)
        {
            var totalWeight = elements.GetTotalWeight();
            return elements.TakeByWeight(weightRate * totalWeight);
        }

        public static WeightedElement<T> TakeWeightedElementByRate<T>(IReadOnlyList<WeightedElement<T>> elements,
            float weightRate)
        {
            var totalWeight = elements.GetTotalWeight();
            return elements.TakeByWeight(weightRate * totalWeight);
        }
    }
}
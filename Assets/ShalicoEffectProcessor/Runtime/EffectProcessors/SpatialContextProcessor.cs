using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [CustomDropdownPath("Context/Spatial/Spatial")]
    [CustomListLabel("Spatial", Tone.Light, HueSymbol.Green)]
    public class SpatialContextProcessor : IEffectProcessor
    {
        [SerializeField] private ModificationType modificationType = ModificationType.Override;
        [SerializeField] private Vector3 position;
        [SerializeField] private Quaternion rotation;
        [SerializeField] private Vector3 scale = Vector3.one;

        public UniTask Run(EffectContext context, EffectFunc function, CancellationToken cancellationToken = default)
        {
            switch (modificationType)
            {
                case ModificationType.Override:
                    context.GetContainer<SpatialInfo>().Value = new SpatialInfo(position, rotation, scale);
                    break;
                case ModificationType.Additive:
                    context.GetContainer<SpatialInfo>().Value += new SpatialInfo(position, rotation, scale);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return function(context, cancellationToken);
        }
    }
}
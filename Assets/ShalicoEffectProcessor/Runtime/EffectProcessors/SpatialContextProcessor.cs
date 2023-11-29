using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using ShalicoEffectProcessor.ContextObjects;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [AddEffectProcessorMenu("Context/Spatial/Spatial")]
    [CustomListLabel("Spatial", Tone.Light, HueSymbol.Green)]
    public class SpatialContextProcessor : IEffectProcessor
    {
        public enum OverrideMode
        {
            Override,
            Add
        }

        [SerializeField] private OverrideMode overrideMode = OverrideMode.Override;
        [SerializeField] private Vector3 position;
        [SerializeField] private Quaternion rotation;
        [SerializeField] private Vector3 scale;

        public UniTask Run(EffectContext context, EffectFunc function, CancellationToken cancellationToken = default)
        {
            switch (overrideMode)
            {
                case OverrideMode.Override:
                    context.SetValue(new SpatialInfo(position, rotation, scale));
                    break;
                case OverrideMode.Add:
                    context.SetValue(new SpatialInfo(
                        context.GetValue<SpatialInfo>().Position + position,
                        context.GetValue<SpatialInfo>().Rotation * rotation,
                        context.GetValue<SpatialInfo>().Scale + scale));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return function(context, cancellationToken);
        }
    }
}
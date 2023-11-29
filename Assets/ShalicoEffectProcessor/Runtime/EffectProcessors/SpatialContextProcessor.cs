using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [AddEffectProcessorMenu("Context/Spatial/Spatial")]
    [CustomListLabel("Spatial", Tone.Light, HueSymbol.Green)]
    public class SpatialContextProcessor : IEffectProcessor
    {
        [SerializeField] private ContextUpdateMode updateMode = ContextUpdateMode.Override;
        [SerializeField] private Vector3 position;
        [SerializeField] private Quaternion rotation;
        [SerializeField] private Vector3 scale;

        public UniTask Run(EffectContext context, EffectFunc function, CancellationToken cancellationToken = default)
        {
            switch (updateMode)
            {
                case ContextUpdateMode.Override:
                    context.SetValue(new SpatialInfo(position, rotation, scale));
                    break;
                case ContextUpdateMode.Additive:
                    var container = context.GetContainer<SpatialInfo>();
                    container.Value = new SpatialInfo(
                        container.Value.Position + position,
                        container.Value.Rotation * rotation,
                        container.Value.Scale + scale);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return function(context, cancellationToken);
        }
    }
}
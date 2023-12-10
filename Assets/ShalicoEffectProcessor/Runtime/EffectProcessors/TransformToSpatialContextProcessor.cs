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
    [CustomDropdownPath("Context/Spatial/Transform To Spatial")]
    [CustomListLabel("Transform To Spatial", Tone.Light, HueSymbol.Green)]
    public class TransformToSpatialContextProcessor : IEffectProcessor
    {
        [SerializeField] private Transform transform;
        [SerializeField] private SpaceType spaceType = SpaceType.World;
        [SerializeField] private ModificationType modificationType = ModificationType.Override;

        public UniTask Run(EffectContext context, EffectFunc function, CancellationToken cancellationToken = default)
        {
            var spatialInfo = new SpatialInfo(transform, spaceType);

            switch (modificationType)
            {
                case ModificationType.Override:
                    context.GetContainer<SpatialInfo>().Value = spatialInfo;
                    break;
                case ModificationType.Additive:
                    context.GetContainer<SpatialInfo>().Value += spatialInfo;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return function(context, cancellationToken);
        }
    }
}
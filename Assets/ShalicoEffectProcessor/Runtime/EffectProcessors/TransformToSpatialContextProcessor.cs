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
    [CustomListLabel(Tone.Light, HueSymbol.Violet)]
    public class TransformToSpatialContextProcessor : UniformEffectProcessor
    {
        [SerializeField] private Transform transform;
        [SerializeField] private SpaceType spaceType = SpaceType.World;
        [SerializeField] private ModificationType modificationType = ModificationType.Override;

        protected override UniTask Run(EffectContext context, CancellationToken cancellationToken)
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

            return UniTask.CompletedTask;
        }
    }
}
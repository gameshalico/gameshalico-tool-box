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
    [CustomListLabel(Tone.Light, HueSymbol.Violet)]
    public class SpatialContextProcessor : UniformEffectProcessor
    {
        [SerializeField] private ModificationType modificationType = ModificationType.Override;
        [SerializeField] private Vector3 position;
        [SerializeField] private Quaternion rotation;
        [SerializeField] private Vector3 scale = Vector3.one;

        protected override UniTask Run(EffectContext context, CancellationToken cancellationToken)
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

            return UniTask.CompletedTask;
        }
    }
}
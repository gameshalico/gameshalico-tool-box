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

        public UniTask Run(EffectContext context, EffectFunc function, CancellationToken cancellationToken = default)
        {
            context.SetValue(new SpatialInfo(transform.position, transform.rotation, transform.localScale));

            return function(context, cancellationToken);
        }
    }
}
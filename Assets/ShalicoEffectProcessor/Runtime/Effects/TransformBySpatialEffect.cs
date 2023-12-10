using System;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.Effects
{
    [Serializable]
    [CustomDropdownPath("Context/Transform By Spatial")]
    [CustomListLabel("Transform By Spatial", Tone.Strong, HueSymbol.Green)]
    public class TransformBySpatialEffect : ImmediateEffect
    {
        [SerializeField] private Transform target;

        [SerializeField] private SpaceType spaceType = SpaceType.World;
        [SerializeField] private ModificationType modificationType = ModificationType.Override;

        protected override void PlayEffectImmediate(EffectContext context)
        {
            var spatial = context.GetContainer<SpatialInfo>().Value;
            spatial.ApplyToTransform(target, spaceType, modificationType);
        }
    }
}
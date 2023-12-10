using System;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShalicoEffectProcessor.Effects
{
    [Serializable]
    [CustomDropdownPath("Game Object/Instantiate")]
    [CustomListLabel("Instantiate", Tone.Strong, HueSymbol.Blue2)]
    public class InstantiateEffect : ImmediateEffect
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parent;
        [SerializeField] private SpaceType spaceType = SpaceType.Local;
        [SerializeField] private bool assignContext;

        protected override void PlayEffectImmediate(EffectContext context)
        {
            var spatial = context.GetContainer<SpatialInfo>().Value;

            var instance = Object.Instantiate(prefab, parent);
            spatial.ApplyToTransform(instance.transform, spaceType, ModificationType.Override);

            if (assignContext) context.GetContainer<GameObject>().Value = instance;
        }
    }
}
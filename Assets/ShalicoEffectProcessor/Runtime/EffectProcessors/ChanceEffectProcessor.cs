using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [CustomDropdownPath("Condition/Chance")]
    [CustomListLabel(Tone.Light, HueSymbol.Green)]
    public class ChanceEffectProcessor : IEffectProcessor
    {
        [Range(0, 1)] [SerializeField] private float chance = 1f;

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            if (Random.value < chance)
                await function(context, cancellationToken);
        }
    }
}
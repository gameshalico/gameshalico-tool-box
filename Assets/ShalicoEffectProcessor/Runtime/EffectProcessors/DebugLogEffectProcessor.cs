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
    [CustomListLabel("Debug Log", Tone.Light, HueSymbol.Yellow)]
    [CustomDropdownPath("Action/Debug Log")]
    public class DebugLogEffectProcessor : IEffectProcessor
    {
        [SerializeField] private string message;

        public UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            Debug.Log(message);
            return function(context, cancellationToken);
        }
    }
}
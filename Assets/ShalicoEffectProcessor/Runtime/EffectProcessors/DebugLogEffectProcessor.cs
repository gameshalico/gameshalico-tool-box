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
    [CustomListLabel(Tone.Light, HueSymbol.Yellow)]
    [CustomDropdownPath("Action/Debug Log")]
    public class DebugLogEffectProcessor : UniformEffectProcessor
    {
        [SerializeField] private string message;

        protected override UniTask Run(EffectContext context, CancellationToken cancellationToken)
        {
            Debug.Log(message);
            return UniTask.CompletedTask;
        }
    }
}
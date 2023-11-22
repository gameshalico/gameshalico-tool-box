using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [CustomListLabel("Debug Log", Tone.Light, HueSymbol.Yellow)]
    [AddEffectProcessorMenu("Action/Debug Log", -1)]
    public class DebugLogEffectProcessor : IEffectProcessor
    {
        [SerializeField] private string message;

        public UniTask Run(Func<CancellationToken, UniTask> function, CancellationToken cancellationToken = default)
        {
            Debug.Log(message);
            return function(cancellationToken);
        }
    }
}
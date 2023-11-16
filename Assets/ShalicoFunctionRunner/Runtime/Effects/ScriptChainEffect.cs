using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShalicoFunctionRunner.Effects
{
    [Serializable]
    [AddEffectMenu("Effects/Script Chain")]
    [CustomListLabel("Script Chain", Tone.Strong, HueSymbol.RedPurple)]
    public class ScriptChainEffect : IEffect
    {
        [FormerlySerializedAs("effectHolder")] [SerializeField]
        private FunctionRunnerHolder functionRunnerHolder;

        public async UniTask PlayEffectAsync(CancellationToken cancellationToken = default)
        {
            await functionRunnerHolder.RunAsync(cancellationToken);
        }
    }
}
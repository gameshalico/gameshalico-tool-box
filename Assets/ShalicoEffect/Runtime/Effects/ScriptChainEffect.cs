using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShalicoEffect.Effects
{
    [Serializable]
    [AddEffectMenu("Effects/Script Chain")]
    [CustomListLabel("Script Chain", Tone.Light, HueSymbol.RedPurple)]
    public class ScriptChainEffect : IEffect
    {
        [FormerlySerializedAs("effectHolder")] [SerializeField]
        private FunctionRunnerHolder functionRunnerHolder;

        public async UniTask PlayEffectAsync(CancellationToken cancellationToken = default)
        {
            await functionRunnerHolder.PlayAsync(cancellationToken);
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.Effects
{
    [Serializable]
    [AddEffectMenu("Effects/Script Chain")]
    [CustomListLabel("Script Chain", Tone.Light, HueSymbol.RedPurple)]
    public class ScriptChainEffect : Effect
    {
        [SerializeField] private EffectHolder effectHolder;

        protected override async UniTask PlayEffectWithTimingAsync(CancellationToken cancellationToken)
        {
            await effectHolder.PlayAsync(cancellationToken);
        }
    }
}
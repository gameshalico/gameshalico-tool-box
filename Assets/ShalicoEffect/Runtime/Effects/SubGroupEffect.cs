using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.Effects
{
    [Serializable]
    [AddEffectMenu("Effects/Sub Group")]
    [CustomListLabel("Sub Group", Tone.Light, HueSymbol.RedPurple)]
    public class SubGroupEffect : Effect
    {
        [SerializeField] private EffectGroup effectGroup;

        protected override async UniTask PlayEffectWithTimingAsync(CancellationToken cancellationToken)
        {
            await effectGroup.PlayAsync(cancellationToken);
        }
    }
}
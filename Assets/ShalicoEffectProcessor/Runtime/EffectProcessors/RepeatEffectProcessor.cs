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
    [CustomDropdownPath("Multiple/Repeat")]
    [CustomListLabel(Tone.Light, HueSymbol.RedPurple)]
    public class RepeatEffectProcessor : IEffectProcessor
    {
        [EnableIf(nameof(repeatForever), false)] [SerializeField]
        private int repeatCount;

        [SerializeField] private bool repeatForever;
        [SerializeField] private bool cloneContext;

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            var repeat = repeatCount + 1;

            do
            {
                var subContext = cloneContext ? context.Clone() : context.AddRef();
                await function(subContext, cancellationToken);
                repeat--;
            } while (repeatForever || repeat > 0);
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Chance")]
    [CustomListLabel("Chance", Tone.Light, HueSymbol.Yellow)]
    public class ChanceFunctionRunner : IFunctionRunner
    {
        [Range(0, 1)] [SerializeField] private float chance = 1f;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            if (Random.value < chance)
                await function(cancellationToken);
        }
    }
}
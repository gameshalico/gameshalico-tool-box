using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffect.FunctionRunners;
using UnityEngine;

namespace ShalicoEffect.Effects
{
    public class EffectHolder : MonoBehaviour
    {
        [SerializeField] private FunctionRunnerChain functionRunnerChain;
        [SerializeField] private EffectGroup effectGroup;

        public virtual async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            await effectGroup.PlayAsync(cancellationToken);
        }
    }
}
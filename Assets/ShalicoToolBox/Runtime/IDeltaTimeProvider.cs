using Cysharp.Threading.Tasks;

namespace ShalicoToolBox
{
    public interface IDeltaTimeProvider
    {
        public float ProvideDeltaTime(PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update);
    }
}
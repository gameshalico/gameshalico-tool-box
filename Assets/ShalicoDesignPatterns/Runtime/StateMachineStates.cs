using System.Collections.Generic;

namespace ShalicoDesignPatterns
{
    public interface IEnterableState<TOwner>
    {
        /// <summary>
        ///     別のステートからこのステートに遷移するときに呼ばれる
        /// </summary>
        /// <param name="prevState">遷移元ステート。初期ステートではnullになる</param>
        void Enter(StateMachine<TOwner>.State prevState);
    }

    public interface IExitableState<TOwner>
    {
        /// <summary>
        ///     このステートが別のステートに遷移するときに呼ばれる
        /// </summary>
        /// <param name="nextState">遷移先ステート</param>
        void Exit(StateMachine<TOwner>.State nextState);
    }

    public interface ITickableState
    {
        /// <summary>
        ///     StateMachineのTickで呼ばれる
        /// </summary>
        /// <param name="deltaTime">経過時間</param>
        public void Tick(float deltaTime);
    }
    
    public partial class StateMachine<TOwner>
    {
        /// <summary>
        ///     ステート
        /// </summary>
        public abstract class State
        {
            private readonly List<ITransition> _tickTransitions = new();
            private readonly Dictionary<int, ITriggerTransition> _transitions = new();

            /// <summary>
            ///　このステートの所属するステートマシン
            /// </summary>
            protected StateMachine<TOwner> StateMachine { get; private set; }

            /// <summary>
            /// ステートを所有するクラスのインスタンス
            /// </summary>
            protected TOwner Owner => StateMachine.Owner;

            internal void SetStateMachine(StateMachine<TOwner> stateMachine)
            {
                StateMachine = stateMachine;
            }

            internal void AddTriggerTransition(ITriggerTransition to)
            {
                _transitions[to.TriggerId] = to;
            }

            internal void AddTickTransition(ITransition to)
            {
                _tickTransitions.Add(to);
            }

            internal bool TryTriggerTransition(int triggerId)
            {
                if (_transitions.TryGetValue(triggerId, out var transition))
                {
                    transition.TryTransition(StateMachine);
                    return true;
                }
                return false;
            }

            internal bool TryTickTransition()
            {
                foreach (var transition in _tickTransitions)
                    if (transition.TryTransition(StateMachine))
                        return true;
                return false;
            }
        }

        /// <summary>
        ///     どの状態からでも遷移可能な状態
        /// </summary>
        private sealed class AnyState : State
        {}
    }
}
using System;

namespace ShalicoDesignPatterns
{
    public partial class StateMachine<TOwner>
    {

        /// <summary>
        ///     イベントによって起動するステート遷移を追加する。
        /// </summary>
        /// <param name="transition">ステート遷移</param>
        /// <typeparam name="TFrom">遷移元のステート型</typeparam>
        public void AddTriggerTransition<TFrom>(ITriggerTransition transition)
            where TFrom : State, new()
        {
            var from = GetOrAddState<TFrom>();
            from.AddTriggerTransition(transition);
        }

        /// <summary>
        ///     イベントによって起動するステート遷移を追加する。
        /// </summary>
        /// <param name="triggerId">遷移するトリガーイベントのID</param>
        /// <typeparam name="TFrom">遷移元のステート型</typeparam>
        /// <typeparam name="TTo">遷移先のステート型</typeparam>
        public void AddTriggerTransition<TFrom, TTo>(int triggerId) where TFrom : State, new() where TTo : State, new()
        {
            AddTriggerTransition<TFrom>(new TriggerTransition(triggerId, GetOrAddState<TTo>()));
        }

        /// <summary>
        ///     イベントによって起動する条件付きのステート遷移を追加する。
        /// </summary>
        /// <param name="triggerId">遷移するトリガーイベントのID</param>
        /// <param name="condition">遷移条件</param>
        /// <typeparam name="TFrom">遷移元のステート型</typeparam>
        /// <typeparam name="TTo">遷移先のステート型</typeparam>
        public void AddTriggerTransition<TFrom, TTo>(int triggerId, Func<StateMachine<TOwner>, bool> condition)
            where TFrom : State, new() where TTo : State, new()
        {
            AddTriggerTransition<TFrom>(new ConditionalTriggerTransition(triggerId, GetOrAddState<TTo>(), condition));
        }

        /// <summary>
        ///     イベントによって起動し、どのステートからでも遷移する遷移を追加する。
        /// </summary>
        /// <param name="triggerId">登録する遷移イベントのID</param>
        /// <typeparam name="TTo">遷移先のステート型</typeparam>
        public void AddTriggerTransitionFromAnyState<TTo>(int triggerId) where TTo : State, new()
        {
            AddTriggerTransition<AnyState, TTo>(triggerId);
        }

        /// <summary>
        ///     イベントによって起動し、どのステートからでも遷移する、条件付きのステート遷移を追加する。
        /// </summary>
        /// <param name="triggerId">登録する遷移イベントのID</param>
        /// <param name="condition">遷移条件</param>
        /// <typeparam name="TTo">遷移先のステート型</typeparam>
        public void AddTriggerTransitionFromAnyState<TTo>(int triggerId, Func<StateMachine<TOwner>, bool> condition)
            where TTo : State, new()
        {
            AddTriggerTransition<AnyState, TTo>(triggerId, condition);
        }

        /// <summary>
        ///     毎Tick判定されるステート遷移を追加する。
        /// </summary>
        /// <param name="transition">ステート遷移</param>
        /// <typeparam name="TFrom">遷移元のステート型</typeparam>
        public void AddTickTransition<TFrom>(ITransition transition)
            where TFrom : State, new()
        {
            var from = GetOrAddState<TFrom>();
            from.AddTickTransition(transition);
        }

        /// <summary>
        ///     毎Tick判定されるステート遷移を追加する。
        /// </summary>
        /// <typeparam name="TFrom">遷移元のステート型</typeparam>
        /// <typeparam name="TTo">遷移先のステート型</typeparam>
        public void AddTickTransition<TFrom, TTo>()
            where TFrom : State, new() where TTo : State, new()
        {
            AddTickTransition<TFrom>(new TrueTransition(GetOrAddState<TTo>()));
        }

        /// <summary>
        ///     毎Tick判定される条件付きのステート遷移を追加する。
        /// </summary>
        /// <param name="condition">ステート遷移</param>
        /// <typeparam name="TFrom">遷移元のステート型</typeparam>
        /// <typeparam name="TTo">遷移先のステート型</typeparam>
        public void AddTickTransition<TFrom, TTo>(Func<StateMachine<TOwner>, bool> condition)
            where TFrom : State, new() where TTo : State, new()
        {
            AddTickTransition<TFrom>(new ConditionalTransition(GetOrAddState<TTo>(), condition));
        }

        /// <summary>
        ///     毎Tick判定される、どのステートからでも遷移する遷移を追加する。
        /// </summary>
        /// <typeparam name="TTo">遷移先のステート型</typeparam>
        public void AddTickTransitionFromAnyState<TTo>()
            where TTo : State, new()
        {
            AddTickTransition<AnyState, TTo>();
        }

        /// <summary>
        ///     毎Tick判定される、どのステートからでも遷移する条件付きの遷移を追加する。
        /// </summary>
        /// <typeparam name="TTo">遷移先のステート型</typeparam>
        public void AddTickTransitionFromAnyState<TTo>(Func<StateMachine<TOwner>, bool> condition)
            where TTo : State, new()
        {
            AddTickTransition<AnyState, TTo>(condition);
        }


        /// <summary>
        ///     ステートの遷移判定を行う
        /// </summary>
        public interface ITransition
        {
            /// <summary>
            ///     遷移先ステート
            /// </summary>
            State To { get; }

            /// <summary>
            ///     遷移可能かどうかを判定する
            /// </summary>
            /// <param name="stateMachine">現在のステートマシン</param>
            /// <returns>遷移可能か</returns>
            bool CanTransition(StateMachine<TOwner> stateMachine);

            internal bool TryTransition(StateMachine<TOwner> stateMachine)
            {
                if (!CanTransition(stateMachine))
                    return true;

                stateMachine.ChangeState(To);
                return false;
            }
        }

        public interface ITriggerTransition : ITransition
        {
            /// <summary>
            ///     判定が行われるトリガーイベントのID
            /// </summary>
            int TriggerId { get; }
        }

        /// <summary>
        ///     必ず遷移する遷移
        /// </summary>
        public class TrueTransition : ITransition
        {
            public TrueTransition(State to)
            {
                To = to;
            }

            public State To { get; }

            public bool CanTransition(StateMachine<TOwner> stateMachine)
            {
                return true;
            }
        }

        /// <summary>
        ///     条件を満たしている場合にのみ遷移する遷移
        /// </summary>
        public class ConditionalTransition : ITransition
        {
            private readonly Func<StateMachine<TOwner>, bool> _condition;

            public ConditionalTransition(State to, Func<StateMachine<TOwner>, bool> condition)
            {
                _condition = condition;
                To = to;
            }

            public State To { get; }

            public bool CanTransition(StateMachine<TOwner> stateMachine)
            {
                return _condition(stateMachine);
            }
        }

        /// <summary>
        ///     必ず遷移する遷移
        /// </summary>
        public class TriggerTransition : TrueTransition, ITriggerTransition
        {
            public TriggerTransition(int triggerId, State to) : base(to)
            {
                TriggerId = triggerId;
            }

            public int TriggerId { get; }
        }

        /// <summary>
        ///     条件を満たしている場合にのみ遷移する遷移
        /// </summary>
        public class ConditionalTriggerTransition : ConditionalTransition, ITriggerTransition
        {
            public ConditionalTriggerTransition(int triggerId, State to, Func<StateMachine<TOwner>, bool> condition) :
                base(to, condition)
            {
                TriggerId = triggerId;
            }

            public int TriggerId { get; }
        }
    }
}
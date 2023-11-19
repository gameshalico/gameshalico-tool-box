using System;
using System.Collections.Generic;

namespace ShalicoToolBox
{
    /// <summary>
    ///     汎用ステートマシン
    /// </summary>
    /// <typeparam name="TOwner">ステートを区別するクラス</typeparam>
    public class StateMachine<TOwner>
    {
        private readonly Dictionary<Type, State> _stateMap = new();

        public StateMachine(TOwner owner)
        {
            Owner = owner;
        }

        public TOwner Owner { get; }
        public State CurrentState { get; private set; }

        public TState AddState<TState>() where TState : State, new()
        {
            TState state = new();
            state.Initialize(this);
            _stateMap.Add(typeof(TState), state);
            return state;
        }

        public State AddState(State state)
        {
            state.Initialize(this);
            _stateMap.Add(state.GetType(), state);
            return state;
        }

        public TState GetOrAddState<TState>() where TState : State, new()
        {
            if (_stateMap.TryGetValue(typeof(TState), out State state))
            {
                return (TState)state;
            }

            return AddState<TState>();
        }

        public void AddTransition<TFrom, TTo>(int triggerId) where TFrom : State, new() where TTo : State, new()
        {
            TFrom from = GetOrAddState<TFrom>();
            TTo to = GetOrAddState<TTo>();
            from.AddTransition(triggerId, to);
        }

        public void AddTransitionFromAnyState<TTo>(int triggerId) where TTo : State, new()
        {
            GetOrAddState<AnyState>().AddTransition(triggerId, GetOrAddState<TTo>());
        }

        public void Start(State first)
        {
            CurrentState = first;
            CurrentState.Enter(null);
        }

        public void Update(float deltaTime)
        {
            if (CurrentState == null)
            {
                throw new InvalidOperationException("State is not started.");
            }

            CurrentState.Update(deltaTime);
        }

        public void Start<TFirst>() where TFirst : State, new()
        {
            Start(GetOrAddState<TFirst>());
        }

        /// <summary>
        ///     トリガーを発行してステートを遷移する
        /// </summary>
        /// <param name="triggerId"></param>
        public void DispatchTrigger(int triggerId)
        {
            if (CurrentState.TryGetTransition(triggerId, out State to) ||
                GetOrAddState<AnyState>().TryGetTransition(triggerId, out to))
            {
                ChangeState(to);
            }
        }

        private void ChangeState(State nextState)
        {
            CurrentState.Exit(nextState);
            nextState.Enter(CurrentState);
            CurrentState = nextState;
        }

        public abstract class State
        {
            private readonly Dictionary<int, State> _transitions = new();
            protected StateMachine<TOwner> StateMachine { get; private set; }
            protected TOwner Owner => StateMachine.Owner;

            internal void Initialize(StateMachine<TOwner> stateMachine)
            {
                StateMachine = stateMachine;
            }

            internal void AddTransition(int triggerId, State to)
            {
                _transitions[triggerId] = to;
            }

            internal bool TryGetTransition(int triggerId, out State to)
            {
                return _transitions.TryGetValue(triggerId, out to);
            }

            internal void Enter(State prevState)
            {
                OnEnter(prevState);
            }

            internal void Update(float deltaTime)
            {
                OnUpdate(deltaTime);
            }

            internal void Exit(State nextState)
            {
                OnExit(nextState);
            }

            protected virtual void OnEnter(State prevState) { }
            protected virtual void OnUpdate(float deltaTime) { }
            protected virtual void OnExit(State nextState) { }
        }

        /// <summary>
        ///     どの状態からでも遷移可能な状態
        /// </summary>
        private sealed class AnyState : State
        {
        }
    }
}
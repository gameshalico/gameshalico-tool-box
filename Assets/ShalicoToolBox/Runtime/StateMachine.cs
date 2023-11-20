using System;
using System.Collections.Generic;

namespace ShalicoToolBox
{
    /// <summary>
    ///     汎用ステートマシン
    /// </summary>
    /// <typeparam name="TOwner">ステートを保持しているクラス</typeparam>
    public partial class StateMachine<TOwner>
    {
        private readonly Dictionary<Type, State> _stateMap = new();

        /// <summary>
        ///     ステートマシンを作成する。
        /// </summary>
        /// <param name="owner">ステートを保持しているクラスのインスタンス</param>
        public StateMachine(TOwner owner)
        {
            Owner = owner;
        }

        /// <summary>
        ///     ステートを保持しているクラスのインスタンス
        /// </summary>
        public TOwner Owner { get; }

        /// <summary>
        ///     現在のステートのインスタンス
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        ///     ステートを追加する
        /// </summary>
        /// <typeparam name="TState">追加するステートの型</typeparam>
        /// <returns>追加したステート</returns>
        public TState AddState<TState>() where TState : State, new()
        {
            TState state = new();
            state.SetStateMachine(this);
            _stateMap.Add(typeof(TState), state);
            return state;
        }

        /// <summary>
        ///     ステートを取得する。存在しない場合は新しく作成する。
        /// </summary>
        /// <typeparam name="TState">取得/登録するステート型</typeparam>
        /// <returns>取得/登録したステート</returns>
        public TState GetOrAddState<TState>() where TState : State, new()
        {
            if (_stateMap.TryGetValue(typeof(TState), out var state))
                return (TState)state;

            return AddState<TState>();
        }

        /// <summary>
        ///     ステートマシンを開始する。
        /// </summary>
        /// <typeparam name="TFirst">初期ステートの型</typeparam>
        public void Start<TFirst>() where TFirst : State, new()
        {
            CurrentState = GetOrAddState<TFirst>();
            if (CurrentState is IEnterableState enterableState)
                enterableState.Enter(null);
        }

        /// <summary>
        ///     ステートマシンを更新する
        /// </summary>
        /// <param name="deltaTime">更新間の経過時間</param>
        /// <exception cref="InvalidOperationException">ステートマシンが開始していない場合</exception>
        public void Tick(float deltaTime)
        {
            ThrowIfStateNotStarted();
            if (CurrentState is ITickableState state)
                state.Tick(deltaTime);

            CurrentState.TryTickTransition();
        }

        /// <summary>
        ///     遷移を発行してステートを遷移する
        /// </summary>
        /// <param name="triggerId">発行する遷移イベントのID</param>
        /// <returns>イベントによりステートが遷移したか</returns>
        /// <exception cref="InvalidOperationException">ステートマシンが開始していない場合</exception>
        public bool DispatchTrigger(int triggerId)
        {
            ThrowIfStateNotStarted();

            if (CurrentState.TryTriggerTransition(triggerId))
                return true;

            if (GetOrAddState<AnyState>().TryTriggerTransition(triggerId))
                return true;

            return false;
        }

        /// <summary>
        ///     キャストして現在のステートを取得する
        /// </summary>
        /// <typeparam name="T">キャストする型</typeparam>
        /// <returns>キャストした現在のステート</returns>
        /// <exception cref="InvalidOperationException">ステートマシンが開始していない場合</exception>
        /// <exception cref="InvalidOperationException">ステートがキャスト出来なかった場合</exception>
        public T GetCurrentStateAs<T>()
        {
            ThrowIfStateNotStarted();
            if (CurrentState is T state)
                return state;
            throw new InvalidOperationException($"Current state is not {typeof(T).Name}.");
        }

        private void ThrowIfStateNotStarted()
        {
            if (CurrentState == null)
                throw new InvalidOperationException("State is not started.");
        }

        private void ChangeState(State nextState)
        {
            if (CurrentState is IExitableState exitableState)
                exitableState.Exit(nextState);
            if (nextState is IEnterableState enterableState)
                enterableState.Enter(CurrentState);
            CurrentState = nextState;
        }
    }
}
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;

namespace ShalicoToolBox
{
    public class PriorityValueArbiter<T>
    {

        private readonly T _defaultValue;

        // 優先度順にソート済みのリスト
        private readonly List<PriorityValueHandler> _priorityValues = new();
        private readonly ReactiveProperty<T> _valueReactiveProperty = new();

        public PriorityValueArbiter(T defaultValue = default)
        {
            _defaultValue = defaultValue;
            _valueReactiveProperty.Value = defaultValue;
        }

        public IReadOnlyReactiveProperty<T> ValueReactiveProperty => _valueReactiveProperty;

        /// <summary>
        ///     優先度付きの値を登録する
        /// </summary>
        /// <param name="priority">優先度。登録されているハンドラのうち、最も高い優先度を持つものの値が採用される</param>
        /// <param name="value">ハンドラの値の初期値</param>
        /// <returns>値を変更するためのハンドラ。Disposeすると登録が破棄される</returns>
        [MustUseReturnValue]
        public IPriorityValueHandler<T> Register(int priority = 0, T value = default)
        {
            var valueHandler = new PriorityValueHandler(this, priority, value);
            InsertHandler(valueHandler);
            UpdateValue();

            return valueHandler;
        }

        private void Remove(PriorityValueHandler priorityValueHandler)
        {
            if (!_priorityValues.Contains(priorityValueHandler))
                return;

            _priorityValues.Remove(priorityValueHandler);
            UpdateValue();
        }

        /// <summary>
        ///     値の変更権を持つハンドラかどうか判定する
        /// </summary>
        /// <param name="priorityValueHandler"></param>
        /// <returns></returns>
        private bool IsTopPriorityValueHandler(PriorityValueHandler priorityValueHandler)
        {
            return _priorityValues[0] == priorityValueHandler;
        }

        /// <summary>
        ///     指定したHandlerの優先度が最も高い場合に値を更新する
        /// </summary>
        /// <param name="priorityValueHandler"></param>
        private void UpdateHandlerValue(PriorityValueHandler priorityValueHandler)
        {
            if (!IsTopPriorityValueHandler(priorityValueHandler))
                return;

            UpdateValue();
        }

        /// <summary>
        ///     指定したHandlerの優先度を更新する
        /// </summary>
        /// <param name="priorityValueHandler"></param>
        private void UpdateHandlerPriority(PriorityValueHandler priorityValueHandler)
        {
            _priorityValues.Remove(priorityValueHandler);
            InsertHandler(priorityValueHandler);
            UpdateHandlerValue(priorityValueHandler);
        }


        /// <summary>
        ///     優先度順にソート済みのリストに挿入する
        /// </summary>
        /// <param name="priorityValueHandler"></param>
        private void InsertHandler(PriorityValueHandler priorityValueHandler)
        {
            var index = 0;
            for (; index < _priorityValues.Count; index++)
                if (priorityValueHandler.Priority > _priorityValues[index].Priority)
                    break;

            _priorityValues.Insert(index, priorityValueHandler);
        }

        private void UpdateValue()
        {
            _valueReactiveProperty.Value = GetTopPriorityValue();
        }


        private T GetTopPriorityValue()
        {
            if (_priorityValues.Count == 0)
                return _defaultValue;

            return _priorityValues[0].Value;
        }


        private class PriorityValueHandler : IPriorityValueHandler<T>
        {
            private readonly PriorityValueArbiter<T> _owner;
            private bool _isDisposed;
            private int _priority;
            private T _value;

            internal PriorityValueHandler(PriorityValueArbiter<T> owner, int priority, T value)
            {
                _owner = owner;
                _priority = priority;
                _value = value;
            }

            public bool IsTopPriority => _owner.IsTopPriorityValueHandler(this);

            public T Value
            {
                get => _value;
                set
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(PriorityValueHandler));
                    _value = value;
                    _owner.UpdateHandlerValue(this);
                }
            }

            public int Priority
            {
                get => _priority;
                set
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(nameof(PriorityValueHandler));
                    if (_priority == value)
                        return;
                    _priority = value;
                    _owner.UpdateHandlerPriority(this);
                }
            }

            public void Dispose()
            {
                _isDisposed = true;
                _owner.Remove(this);
            }
        }
    }
}
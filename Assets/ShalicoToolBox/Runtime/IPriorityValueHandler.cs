using System;

namespace ShalicoToolBox
{
    /// <summary>
    ///     優先度付きの値を管理するインターフェース
    /// </summary>
    /// <typeparam name="T">値の型</typeparam>
    public interface IPriorityValueHandler<T> : IDisposable
    {
        /// <summary>
        ///     値
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        ///     優先度
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        ///     優先度が最も高いかどうか
        /// </summary>
        /// +
        public bool IsTopPriority { get; }
    }
}
using System;
using NUnit.Framework;
using ShalicoToolBox;
using UnityEngine;

namespace Tests
{
    public class EasingFunctionsTest
    {
        private const float Epsilon = 0.01f;

        [Test]
        public void 値チェック()
        {
            foreach (var easeTypeObject in Enum.GetValues(typeof(EaseType)))
            {
                var easeType = (EaseType)easeTypeObject;
                Debug.Log(easeType);
                Assert.That(EasingFunctions.Ease(easeType, 0.0f), Is.EqualTo(0.0f).Within(Epsilon));
                Assert.That(EasingFunctions.Ease(easeType, 0.5f), Is.Not.EqualTo(float.NaN));
                Assert.That(EasingFunctions.Ease(easeType, 1.0f), Is.EqualTo(1.0f).Within(Epsilon));
            }
        }
    }
}
using System;
using UniRx;

namespace ShalicoToolBox
{
    public class HierarchyScaler
    {
        private readonly ReactiveProperty<float> _scaleReactiveProperty = new();
        private float _inheritScale;

        private HierarchyScaler _parentHierarchyScaler;
        private IDisposable _parentSubscription;

        private float _selfScale;

        public HierarchyScaler(HierarchyScaler parentHierarchyScaler = null, float selfScale = 1f)
        {
            SelfScale = selfScale;
            SetParent(parentHierarchyScaler);
        }

        public float SelfScale
        {
            get => _selfScale;
            set
            {
                _selfScale = value;
                UpdateCurrentScale();
            }
        }

        public IReadOnlyReactiveProperty<float> ScaleReactiveProperty => _scaleReactiveProperty;

        public void SetParent(HierarchyScaler parentHierarchyScaler)
        {
#if DEBUG
            if (IsCircularReference(parentHierarchyScaler))
                throw new ArgumentException("Circular reference detected");
#endif
            _parentSubscription?.Dispose();
            _parentHierarchyScaler = parentHierarchyScaler;

            if (parentHierarchyScaler == null)
            {
                _inheritScale = 1f;
                return;
            }

            _parentSubscription = parentHierarchyScaler.ScaleReactiveProperty
                .Subscribe(scale =>
                {
                    _inheritScale = scale;
                    UpdateCurrentScale();
                });
        }

        private bool IsCircularReference(HierarchyScaler parentHierarchyScaler)
        {
            var hierarchyScaler = parentHierarchyScaler;
            do
            {
                if (hierarchyScaler == this)
                    return true;
                hierarchyScaler = hierarchyScaler?._parentHierarchyScaler;
            } while (hierarchyScaler != null);

            return false;
        }

        private void UpdateCurrentScale()
        {
            _scaleReactiveProperty.Value = _inheritScale * SelfScale;
        }
    }
}
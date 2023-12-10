using System;
using UnityEngine;

namespace ShalicoEffectProcessor.Context
{
    public readonly struct SpatialInfo
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly Vector3 Scale;

        public SpatialInfo(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }

        public SpatialInfo(Transform transform, SpaceType spaceType)
        {
            switch (spaceType)
            {
                case SpaceType.World:
                    Position = transform.position;
                    Rotation = transform.rotation;
                    Scale = transform.lossyScale;
                    break;
                case SpaceType.Local:
                    Position = transform.localPosition;
                    Rotation = transform.localRotation;
                    Scale = transform.localScale;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spaceType), spaceType, null);
            }
        }

        private Vector3 LossyScale(Transform transform, Vector3 scale)
        {
            var lossyScale = transform.lossyScale;
            return new Vector3(
                scale.x / lossyScale.x * scale.x,
                scale.y / lossyScale.y * scale.y,
                scale.z / lossyScale.z * scale.z
            );
        }

        public static SpatialInfo operator +(SpatialInfo a, SpatialInfo b)
        {
            return new SpatialInfo(
                a.Position + b.Position,
                a.Rotation * b.Rotation,
                a.Scale + b.Scale
            );
        }

        private void SetToTransform(Transform target, SpaceType spaceType)
        {
            target.position = Position;
            target.rotation = Rotation;
            switch (spaceType)
            {
                case SpaceType.World:
                    target.localScale = LossyScale(target, Scale);
                    break;
                case SpaceType.Local:
                    target.localScale = Scale;
                    break;
            }
        }


        public void ApplyToTransform(Transform target, SpaceType spaceType, ModificationType modificationType)
        {
            switch (modificationType)
            {
                case ModificationType.Override:
                    SetToTransform(target, spaceType);
                    break;
                case ModificationType.Additive:
                    var spatial = new SpatialInfo(target, spaceType);
                    (spatial + this).SetToTransform(target, spaceType);
                    break;
            }
        }
    }
}
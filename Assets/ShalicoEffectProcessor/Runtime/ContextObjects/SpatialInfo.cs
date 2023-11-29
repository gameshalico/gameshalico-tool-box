using UnityEngine;

namespace ShalicoEffectProcessor.ContextObjects
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
    }
}
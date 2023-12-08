using System;
using ShalicoAttributePack;
using UnityEngine;

namespace Samples.AttributePack
{
    [Serializable]
    [CustomDropdownPath("Enemies/Green", "ゴブリン")]
    public class Goblin : IEntity
    {
        [SerializeField] private int health;
        [SerializeField] private int attack;
    }
}
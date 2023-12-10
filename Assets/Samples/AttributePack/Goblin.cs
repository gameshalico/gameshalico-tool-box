using System;
using ShalicoAttributePack;
using UnityEngine;

namespace Samples.AttributePack
{
    [Serializable]
    [CustomDropdownPath("Enemies/Green/Goblin")]
    public class Goblin : IEntity
    {
        [SerializeField] private int health;
        [SerializeField] private int attack;
    }
}
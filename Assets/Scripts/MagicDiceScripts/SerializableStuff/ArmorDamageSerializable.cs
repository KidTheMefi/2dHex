using System;
using EffectDealers;
using UnityEngine;

namespace SerializableStuff
{
    [Serializable]
    public class ArmorDamageSerializable
    {
        [SerializeField]
        public PhysicDamage armorType;
        [SerializeField]
        public int value;
    }
}
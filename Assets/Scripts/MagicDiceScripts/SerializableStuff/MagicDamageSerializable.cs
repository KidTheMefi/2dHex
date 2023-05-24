using System;
using EffectDealers;
using UnityEngine;

namespace SerializableStuff
{
    [Serializable]
    public class MagicDamageSerializable
    {
        [SerializeField]
        public MagicDamage magicType;
        [SerializeField]
        public int value;
    }
}
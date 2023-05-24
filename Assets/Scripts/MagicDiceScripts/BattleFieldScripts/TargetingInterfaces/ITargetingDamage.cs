using System;
using EffectDealers;
using UnityEngine;

namespace BattleFieldScripts
{
    public interface ITargetingDamage
    {
        public event Action<Vector2Int, IEffectDealer> ShowTargetEffect;
    }
}
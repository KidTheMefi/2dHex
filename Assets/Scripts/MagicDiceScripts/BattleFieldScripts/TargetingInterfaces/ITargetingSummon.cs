using System;
using CharactersScripts;
using UnityEngine;

namespace BattleFieldScripts
{
    public interface ITargetingSummon
    {
        public event Action<Vector2Int, Character> ShowTargetSummon;
        public void UndoSummon();
    }
}
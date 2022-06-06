using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "HexGame/EnemiesSetting")]
    public class EnemiesSettings : ScriptableObject
    {
        [SerializeField]
        private List<EnemySettings> _enemies;
    
        public List<EnemySettings> Enemies => _enemies;
    }
}

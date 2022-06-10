using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "HexGame/EnemySettings")]
    public class EnemySettings : ScriptableObject
    {
        [SerializeField]
        private EnemyMapModel.Properties _properties;
        
        public EnemyMapModel.Properties Properties => _properties;
    }
}

using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "HexGame/EnemySettings")]
    public class EnemySettings : ScriptableObject
    {
        [SerializeField]
        private EnemyModel.Properties _properties;
        
        public EnemyModel.Properties Properties => _properties;
    }
}

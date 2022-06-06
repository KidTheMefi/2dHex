using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(menuName = "HexGame/EnemySettings")]
    public class EnemySettings : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _maxEnergy;
        [SerializeField] private int _hp;
        [SerializeField] private int _attack;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private BiomType _biomSpawn;

    public Sprite Sprite => _sprite;
    public int MaxEnergy => _maxEnergy;
    public int HP => _hp;
    public int Attack => _attack;
    public string EnemyName => _name;
    public string Description => _description;
    public BiomType BiomSpawn => _biomSpawn;
    }
}

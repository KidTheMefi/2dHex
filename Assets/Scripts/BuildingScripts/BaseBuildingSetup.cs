using UnityEngine;

namespace BuildingScripts
{
    [CreateAssetMenu(menuName = "Buildings/BaseBuildingSetup")]
    public class BaseBuildingSetup : ScriptableObject
    {
        public Sprite sprite;
        public string name;
        public string description;
        public LandType landType;
        public BiomType landBiom;
    }
}
using System.Collections.Generic;
using CharactersScripts.CharactersSkill;
using EffectDealers;
using SerializableStuff;
using UnityEngine;

namespace ScriptableScripts
{
    [CreateAssetMenu(menuName = "Character/SomeCharacter", fileName = "Character")]
    public class CharacterScriptable : ScriptableObject
    {
        public string CharacterName;
        public bool SummonedCreature;
        public Animator CharacterAnimatorPrefab;
        public Sprite CharacterSprite;
        [Range(1, 10)]
        public int HP;
        [Range(0, 10)]
        public int Armor;
        [Range(0, 10)]
        public int MagicShield;
        [SerializeField] public int HireCost = 4;
        public int MoneyOnDeath => HireCost / 2; //- 2 > 0 ? HireCost - 2 : 0;
        [SerializeField]
        private List<MagicDamageSerializable> _magicDefenceList;
        [SerializeField]
        private List<ArmorDamageSerializable> _armorDefenceList;

        public virtual ICharacterAttack GetAttack()
        {
            return null;
        }

        public Dictionary<MagicDamage, int> GetMagicDefence()
        {
            return MagicListToDictionary(_magicDefenceList);
        }
        public Dictionary<PhysicDamage, int> GetArmorDefence()
        {
            return PhysicListToDictionary(_armorDefenceList);
        }
        
        
        protected Dictionary<MagicDamage, int> MagicListToDictionary(List<MagicDamageSerializable> magicDamageList)
        {
            Dictionary<MagicDamage, int> magicDefenceDictionary = new Dictionary<MagicDamage, int>();

            foreach (var magicDefence in magicDamageList)
            {
                if (magicDefenceDictionary.ContainsKey(magicDefence.magicType))
                {
                    magicDefenceDictionary[magicDefence.magicType] += magicDefence.value;
                }
                else
                {
                    magicDefenceDictionary.Add(magicDefence.magicType, magicDefence.value);
                }
            }
            return magicDefenceDictionary;
        }

        protected Dictionary<PhysicDamage, int> PhysicListToDictionary(List<ArmorDamageSerializable> armorDefenceList)
        {
            Dictionary<PhysicDamage, int> magicDefenceDictionary = new Dictionary<PhysicDamage, int>();

            foreach (var magicDefence in armorDefenceList)
            {
                if (magicDefenceDictionary.ContainsKey(magicDefence.armorType))
                {
                    magicDefenceDictionary[magicDefence.armorType] += magicDefence.value;
                }
                else
                {
                    magicDefenceDictionary.Add(magicDefence.armorType, magicDefence.value);
                }
            }
            return magicDefenceDictionary;
        }

        private void OnEnable()
        {
            CharacterName = name;
        }
        private void OnValidate()
        {
            CharacterName = name;
        }
    }
}
using System.Collections.Generic;
using CharactersScripts.CharactersSkill;
using EffectDealers;
using SerializableStuff;
using UnityEngine;

namespace ScriptableScripts.CharactersScriptable
{
    [CreateAssetMenu(menuName = "Character/RandomAttackCharacter", fileName = "RandomAttackCharacter")]
    public class RandomTargetAttack : CharacterScriptable
    {
        [SerializeField]
        private List<MagicDamageSerializable> _magicDamageList;
        [SerializeField]
        private List<ArmorDamageSerializable> _physicDamageList;
    
        public override ICharacterAttack GetAttack()
        {
            DamageDealer damageDealer = new DamageDealer(MagicListToDictionary(_magicDamageList), PhysicListToDictionary(_physicDamageList) );

            return new CharacterRandomAttack(damageDealer);
        }
    }
}
using System.Collections.Generic;
using CharactersScripts.CharactersSkill;
using EffectDealers;
using SerializableStuff;
using UnityEngine;

namespace ScriptableScripts.CharactersScriptable
{
    [CreateAssetMenu(menuName = "Character/MeleeWideAttackCharacter", fileName = "MeleeWideAttackCharacter")]
    public class MeleeWideAttackCharacter : CharacterScriptable
    {
        [SerializeField]
        private List<MagicDamageSerializable> _magicDamageList;
        [SerializeField]
        private List<ArmorDamageSerializable> _physicDamageList;
    
        public override ICharacterAttack GetAttack()
        {
            DamageDealer damageDealer = new DamageDealer(MagicListToDictionary(_magicDamageList), PhysicListToDictionary(_physicDamageList) );

            return new CharacterMeleeWideAttack(damageDealer);
        }
    }
}
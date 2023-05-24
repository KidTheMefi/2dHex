using System.Collections.Generic;
using CharactersScripts.CharactersSkill;
using EffectDealers;
using SerializableStuff;
using UnityEngine;

namespace ScriptableScripts
{
    [CreateAssetMenu(menuName = "Character/SimpleCloseAttackCharacter", fileName = "CloseAttackCharacter")]
    public class SimpleCloseAttackCharacterScriptable : CharacterScriptable
    {
        [SerializeField]
        private List<MagicDamageSerializable> _magicDamageList;
        [SerializeField]
        private List<ArmorDamageSerializable> _physicDamageList;
    
        public override ICharacterAttack GetAttack()
        {
            DamageDealer damageDealer = new DamageDealer(MagicListToDictionary(_magicDamageList), PhysicListToDictionary(_physicDamageList) );

            return new CharacterCloseAttack(damageDealer);
        }
    }
}
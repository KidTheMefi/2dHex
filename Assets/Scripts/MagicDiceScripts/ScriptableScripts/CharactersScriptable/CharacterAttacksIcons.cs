using System;
using UnityEngine;

namespace ScriptableScripts
{

    public static class CharacterAttackIcon
    {
        private static CharacterAttacksIcons _instance = GetFromResources();
        public static CharacterAttacksIcons Instance => _instance;
        static CharacterAttacksIcons GetFromResources()
        {
            return Resources.Load<CharacterAttacksIcons>("ScriptableObjects/AttackIcons");
        }
    }
    
    [CreateAssetMenu (menuName = "AttackIcons")]
    public class CharacterAttacksIcons : ScriptableObject
    {
        public Sprite CloseAttackSprite;
        public Sprite DistanceAttackSprite;
        public Sprite DistanceOnPointAttackSprite;
        public Sprite AllInLineAttackSprite;
        public Sprite FirstInLineAttackSprite;
        public Sprite MelleWideAttackSprite;
    }
}
using System;
using BattleFieldScripts;
using CharactersScripts.CharactersSkill;
using DefaultNamespace;
using EffectDealers;
using ScriptableScripts;
using TMPro;
using UnityEngine;



namespace CharactersScripts
{
    public class Character : MonoBehaviour
    {
        public Vector2Int PositionOnBattleField { private set; get; }
        public event Action<Character> CharacterRemoveEvent = delegate{ };
        [SerializeField]
        private SpriteRenderer _characterSprite;
        [SerializeField]
        private ArmorHandler _armorHandler;
        [SerializeField]
        private MagicDefenceHandler _magicDefenceHandler;
        [SerializeField]
        private HealthHandler _healthHandler;
        private DamageReceiver _damageReceiver;
        private PositiveEffectReceiver _positiveEffectReceiver;
        [SerializeField]
        private TextMeshPro _nameTextMeshPro;
        [SerializeField]
        private CharacterAttackHandler _characterAttackHandler;
        [SerializeField]
        private CharacterBody _characterBody;
        public CharacterAttackHandler CharacterAttackHandler => _characterAttackHandler;
        public PositiveEffectReceiver PositiveEffectReceiver => _positiveEffectReceiver;
        public DamageReceiver DamageReceiver => _damageReceiver;
        public CharacterEffectHandler CharacterEffectHandler { private set; get; }
        public CharacterScriptable CharacterScriptable { private set; get; }
        private bool _haveReward;

        public string CharacterDescription()
        {
            var reward = _haveReward ? $"Reward: {CharacterScriptable.MoneyOnDeath}" : "";
            string description = $"{_nameTextMeshPro.text } {reward}\n" +
                $"{_armorHandler.GetArmorDefenceDescription()}" +
                $"{_magicDefenceHandler.GetMagicDefenceDescription()}" +
                $"{_characterAttackHandler.Description()}" +
                ShieldDescription();

            return description;
        }

        private string ShieldDescription()
        {
            string shieldDescription = null;
            if (CharacterEffectHandler.TryGetEffectTypeOf(typeof(Shield), out var effect))
            {
                shieldDescription += effect.Description();
            }
            return shieldDescription;
        }
        
        public void Setup(CharacterScriptable characterScriptable)
        {
            CharacterScriptable = characterScriptable;
            CharacterEffectHandler = new CharacterEffectHandler();
            _characterBody.SetBody(characterScriptable.CharacterAnimatorPrefab);
            _armorHandler.Setup(CharacterEffectHandler, characterScriptable.Armor, characterScriptable.GetArmorDefence());
            _magicDefenceHandler.Setup(CharacterEffectHandler, characterScriptable.MagicShield, characterScriptable.GetMagicDefence());
            _healthHandler.SetupHealth(characterScriptable.HP,characterScriptable.HP, CharacterDeath);
            _damageReceiver = new DamageReceiver(_armorHandler, _magicDefenceHandler, _healthHandler);
            _positiveEffectReceiver = new PositiveEffectReceiver(_armorHandler, _magicDefenceHandler, _healthHandler);
            _characterAttackHandler.SetAttack(characterScriptable.GetAttack());
            _characterSprite.sprite = characterScriptable.CharacterSprite;
            
            SetName(characterScriptable.CharacterName);
        }

        private void SetName(string characterName)
        {
            name = characterName;
            _nameTextMeshPro.text = characterName;
        }

        public void SetPositionOnBattleField(Vector2Int pos, TeamField currentField)
        {
            PositionOnBattleField = pos;
            _characterAttackHandler.SetPosition(pos,currentField);

            _haveReward = !currentField.PlayerField;

            if (currentField.PlayerField)
            {
                _characterBody.transform.localScale = new Vector3(-Mathf.Abs(_characterBody.transform.localScale.x), _characterBody.transform.localScale.y);
            }
        }

        public void ConfirmValuesChange(bool value)
        {
            _healthHandler.ConfirmHealthValue(value);
            _armorHandler.ConfirmValue(value);
            _magicDefenceHandler.ConfirmDamage(value);
            _characterAttackHandler.ConfirmAdditionalDamage(value);
            CharacterEffectHandler.ConfirmEffect(value);
            if (value)
            {
                _characterBody.Hurt();
            }
        }

        public void CharmDamage(DamageDealer damageDealer)
        {
            _characterAttackHandler.AddAdditionalDamage(damageDealer);
        }

        public void RemoveCharacter()
        {
            CharacterRemoveEvent.Invoke(this);
            gameObject.SetActive(false);
        }
        private void CharacterDeath()
        {
            if (_haveReward)
            {
                ProgressionHandler.GetInstance().ChangeMoneyValueBy(CharacterScriptable.MoneyOnDeath);
            }
            RemoveCharacter();
        }
    }
}
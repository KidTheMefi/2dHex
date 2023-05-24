using System.Collections.Generic;
using CharactersScripts;
using DamageDisplayScripts;
using EffectDealers;
using SpellFormScripts;
using UnityEngine;

namespace BattleFieldScripts
{
    public class TargetingHandler
    {
        private TeamField _teamField;
        private List<BattleFieldPosition> EffectedBattleFieldPosition = new List<BattleFieldPosition>();
        private List<BattleFieldPosition> SummonedInBattleFieldPosition = new List<BattleFieldPosition>();
        private PossibleTarget _possibleTarget;
        private TargetingForm _targetingForm;
        
        public TargetingHandler(TargetingForm targetingForm, TeamField teamField, PossibleTarget possibleTarget)
        {
            _teamField = teamField;
            _targetingForm = targetingForm;
            _possibleTarget = possibleTarget;
            if (_teamField == null)
            {
                throw new System.ArgumentNullException();
            }
            SubscribeToDisplay(true);
        }

        private void SubscribeToDisplay(bool value)
        {
            if (value)
            {
                if (_targetingForm is ITargetingDamage iTargetingDamage)
                {
                    iTargetingDamage.ShowTargetEffect += PositionEffected;
                }
                if (_targetingForm is ITargetingSummon iTargetingSummon)
                {
                    iTargetingSummon.ShowTargetSummon += PositionSummon;
                }
            }
            else
            {
                if (_targetingForm is ITargetingDamage iTargetingDamage)
                {
                    iTargetingDamage.ShowTargetEffect -= PositionEffected;
                }
                if (_targetingForm is ITargetingSummon iTargetingSummon)
                {
                    iTargetingSummon.ShowTargetSummon -= PositionSummon;
                }
            }
        }

        public void SetPossibleTargets()
        {
            _teamField.SetPossibleTargets(new TargetingPossibility(_teamField).GetPossibleTargets(_possibleTarget));
        }
        public void ShowEffect(BattleFieldPosition battleFieldPosition)
        {
            EffectedBattleFieldPosition.Clear();
            ShowEffectedPosition(_teamField.GetBattlePositionVector(battleFieldPosition));
        }

        private void ShowEffectedPosition(Vector2Int positionInFieldArray)
        {
            _targetingForm.ShowEffectedPosition(positionInFieldArray);
        }

        private void PositionEffected(Vector2Int position, IEffectDealer effectDealer)
        {
            if (_teamField.TryGetPositionFromArray(position, out var battleFieldPosition))
            {
                battleFieldPosition.Effected(true);
                EffectedBattleFieldPosition.Add(battleFieldPosition);
                if (!battleFieldPosition.IsEmpty())
                {
                    effectDealer.DealEffectTo(battleFieldPosition.CharacterOnPosition, true);
                }
            }
        }

        private void PositionSummon(Vector2Int position, Character character)
        {
            if (_teamField.TryGetPositionFromArray(position, out var battleFieldPosition) && battleFieldPosition.IsEmpty())
            {
                battleFieldPosition.Effected(true);
                SummonedInBattleFieldPosition.Add(battleFieldPosition);

                //Debug.Log($"position summoned {character.name}");
                character.gameObject.SetActive(true);
                character.SetPositionOnBattleField(position, _teamField);
                battleFieldPosition.AddCharacter(character);
            }
            else
            {
                character.RemoveCharacter();
            }
        }

        public void ConfirmAction()
        {
            foreach (var position in EffectedBattleFieldPosition)
            {
                position.Effected(false);
                if (!position.IsEmpty())
                {
                    position.CharacterOnPosition.ConfirmValuesChange(true);
                }
            }
            SubscribeToDisplay(false);
            DamageDisplaySignals.GetInstance().InvokeHideDamage();
        }
        public void HideActionEffects(bool canceled)
        {
            DamageDisplaySignals.GetInstance().InvokeHideDamage();
            if (canceled)
            {
                SubscribeToDisplay(false);
                if (_targetingForm is ITargetingSummon summonAllySpell)
                {
                    summonAllySpell.UndoSummon();
                }
            }

            foreach (var position in EffectedBattleFieldPosition)
            {
                position.Effected(false);
                if (!position.IsEmpty())
                {
                    position.CharacterOnPosition.ConfirmValuesChange(false);
                }
            }

            foreach (var position in SummonedInBattleFieldPosition)
            {
                if (!position.IsEmpty())
                {
                    var character = position.CharacterOnPosition;
                    position.RemoveFromPosition(character);
                    character.RemoveCharacter(); // rework
                }
            }
        }
    }
}
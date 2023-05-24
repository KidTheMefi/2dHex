using System.Collections.Generic;
using System.Threading;
using BattleFieldScripts;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MagicSkills
{
    public class MagicSpellHandler
    {
        private TargetingHandler _targetingHandler;
        private CancellationTokenSource _cancellationTokenSource;
        private TeamField _teamField;

        private bool _isSubscribed;

        public MagicSpellHandler(IMagicSpell magicSpell)
        {
            _teamField = BattleField.Instance.GetField(magicSpell.GetField());
            _targetingHandler = new TargetingHandler(magicSpell.GetTargetingForm(), _teamField, magicSpell.GetPossibleTarget());
        }
        
        public MagicSpellHandler(IMagicSpell magicSpell, TeamField teamField)
        {
            _teamField = teamField;
            _targetingHandler = new TargetingHandler(magicSpell.GetTargetingForm(), _teamField, magicSpell.GetPossibleTarget());
        }

        public async UniTask<bool> ExecuteSpellAsync(CancellationToken cancelSpell)
        {
            if (_targetingHandler == null)
            {
                throw new System.ArgumentException("Spell has no TargetingForm");
            }
            await UniTask.Yield();
            _targetingHandler.SetPossibleTargets();

            SubscribeToTeamFieldSignals(true);
            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.WaitUntil(() => _cancellationTokenSource.IsCancellationRequested || cancelSpell.IsCancellationRequested);
            SubscribeToTeamFieldSignals(false);
            if (cancelSpell.IsCancellationRequested)
            {
                Canceled();
                return false;
            }
            return true;
        }

        private void SubscribeToTeamFieldSignals(bool value)
        {
            if (_isSubscribed == value)
            {
                return;
            }
            
            if (value)
            {
                _teamField.Confirm += EventsOnConfirm;
                _teamField.MouseExitPosition += EventsOnMouseExitPosition;
                _teamField.MouseOnPosition += EventsOnMouseOnPosition;
            }
            else
            {
                _teamField.Confirm -= EventsOnConfirm;
                _teamField.MouseExitPosition -= EventsOnMouseExitPosition;
                _teamField.MouseOnPosition -= EventsOnMouseOnPosition;
            }
            _isSubscribed = value;
        }

        private void EventsOnMouseOnPosition(BattleFieldPosition position)
        {
            _targetingHandler.ShowEffect(position);
        }
        private void EventsOnMouseExitPosition()
        {
            _targetingHandler.HideActionEffects(false);
        }
        private void EventsOnConfirm()
        {
            _targetingHandler.ConfirmAction();
            _cancellationTokenSource.Cancel();
        }

        private  void Canceled()
        {
            _targetingHandler.HideActionEffects(true);
            _teamField.SetPossibleTargets(new List<Vector2Int>());
        }
        
        
    }
}
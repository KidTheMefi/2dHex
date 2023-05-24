using CharactersScripts;
using UnityEngine;

namespace BattleFieldScripts
{
    public enum PositionState
    {
        Default, CanBeTarget, Target
    }
    public class BattleFieldPosition : MonoBehaviour
    {
        public Character CharacterOnPosition { private set; get; }

        [SerializeField]
        private SpriteRenderer _backRenderer;
        [SerializeField]
        private SpriteRenderer _effectedRenderer;
        [SerializeField]
        private Color _defaultColor;
        private PositionState _positionState;
        
        private void Awake()
        {
            _defaultColor = _backRenderer.color;
        }
        public void AddCharacter(Character character)
        {
            if (character == null)
            {
                return;
            }
            
            CharacterOnPosition = character;
            CharacterOnPosition.CharacterRemoveEvent += RemoveFromPosition;
            character.transform.SetParent(transform);
            character.transform.localPosition = Vector3.zero;
        }

        
        public bool IsEmpty()
        {
            return CharacterOnPosition == null;
        }

        public void RemoveFromPosition(Character character)
        {
            if (CharacterOnPosition == null || CharacterOnPosition != character) return;
            CharacterOnPosition.CharacterRemoveEvent -= RemoveFromPosition;
            CharacterOnPosition = null;
        }
        
        public void Effected(bool value)
        {
            _effectedRenderer.gameObject.SetActive(value);
        }
        public void ChangeState(PositionState positionState)
        {
            switch (positionState)
            {
                case PositionState.Default:
                    _backRenderer.color = _defaultColor;
                    break;
                case PositionState.CanBeTarget:
                    _backRenderer.color = new Color(0, 0.3f, 0.5f, 0.5f);
                    break;
                case PositionState.Target:
                    _backRenderer.color = new Color(0.7f, 0.2f, 0.2f, 0.5f);
                    break;
            }
            _positionState = positionState;
        }

        private void OnMouseEnter()
        {
            if (CharacterOnPosition != null)
            {
                BattleFieldPositionSignal.GetInstance().InvokeMouseEnterCharacter(CharacterOnPosition);
            }

            if (_positionState == PositionState.CanBeTarget)
            {
                BattleFieldPositionSignal.GetInstance().InvokeMouseOnPosition(this);
            }
        }

        private void OnMouseDown()
        {
            if (_positionState == PositionState.Target)
            {
               BattleFieldPositionSignal.GetInstance().InvokeSelectPosition(this);
               
               if (CharacterOnPosition != null)
               {
                   BattleFieldPositionSignal.GetInstance().InvokeMouseEnterCharacter(CharacterOnPosition);
               }
            }
        }

        private void OnMouseExit()
        {
            BattleFieldPositionSignal.GetInstance().InvokeMouseExitPosition();
            BattleFieldPositionSignal.GetInstance().InvokeMouseExitCharacter();
        }
    }
}
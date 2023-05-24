using System;
using System.Collections.Generic;

namespace CharactersScripts
{
    public class CharacterEffectHandler
    {
        private List<ICharacterEffect> _effects = new List<ICharacterEffect>();
        private List<ICharacterEffect> _temporaryEffect = new List<ICharacterEffect>();

        private List<ICharacterEffect> _effectToRemove = new List<ICharacterEffect>();
        public void ConfirmEffect(bool value)
        {
            if (!value)
            {
                var tempEffects = SomeStatic.ListToQueue(_temporaryEffect);

                while (tempEffects.Count !=0)
                {
                    var tempEffect = tempEffects.Dequeue();
                    _effects.Remove(tempEffect);
                }

                foreach (var removedEffect in _effectToRemove)
                {
                    _effects.Add(removedEffect);
                }
            }
            
            _temporaryEffect.Clear();
            _effectToRemove.Clear();
        }



        public void AddCharacterEffect(ICharacterEffect characterEffect)
        {
            if (TryGetEffectTypeOf(_temporaryEffect, characterEffect.GetType(), out var temporaryEffect)  )
            {
                _temporaryEffect.Remove(temporaryEffect);
                
                if (!_effectToRemove.Contains(temporaryEffect))
                {
                    _effectToRemove.Add(temporaryEffect);
                }
                
            }
            if (TryGetEffectTypeOf(_effects, characterEffect.GetType(), out var effect))
            {
                _effects.Remove(effect);
                if (!_effectToRemove.Contains(effect))
                {
                    _effectToRemove.Add(effect);
                }
            }
            
            _effects.Add(characterEffect);
            _temporaryEffect.Add(characterEffect);
            
        }


        public bool TryGetEffectTypeOf(Type typeOfCharacterEffect, out ICharacterEffect effect)
        {
            return TryGetEffectTypeOf(_effects, typeOfCharacterEffect, out effect);
        }
        
        private bool TryGetEffectTypeOf(List<ICharacterEffect> effects, Type typeOfCharacterEffect, out ICharacterEffect effect)
        {
            if (!typeof(ICharacterEffect).IsAssignableFrom(typeOfCharacterEffect))
            {
                effect = null;
                return false;
            }

            foreach (var characterEffect in effects)
            {
                if (characterEffect.GetType() == typeOfCharacterEffect)
                {
                    effect = characterEffect;
                    return true;
                }
            }
            effect = null;
            return false;
        }
    }
}
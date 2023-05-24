using BattleFieldScripts;
using EffectDealers;
using SpellFormScripts;

namespace MagicSkills.WindBasedSpells
{
    public class WindCharm  : SinglePositiveSpellOnAlly
        {
            public WindCharm()
            {
                positiveDealer = new WeaponCharmEffectDealer(MagicDamage.Wind, 1);
                form = new SingleTargetForm(positiveDealer);
            }

            public override PossibleTarget GetPossibleTarget()
            {
                return PossibleTarget.AnyPositionWithCharacter;
            }
            public override string[] GetCombination()
            {
                return new[] { "Wind" };
            }
        }
    }

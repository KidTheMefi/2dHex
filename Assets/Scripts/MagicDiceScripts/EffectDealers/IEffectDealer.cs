using CharactersScripts;

namespace EffectDealers
{
    public interface IEffectDealer
    {
        public void DealEffectTo(Character character, bool displayDamage = false);
    }
}
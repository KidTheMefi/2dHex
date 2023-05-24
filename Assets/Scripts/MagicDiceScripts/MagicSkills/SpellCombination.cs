using System.Linq;

namespace MagicSkills
{
    public class SpellCombination
    {
        private string[] _combination;
        
        public SpellCombination(string[] combination)
        {
            _combination = combination;
        }

        public bool ContainsRune(string runeName)
        {
            return _combination.Any(rune => rune == runeName);
        }

        public bool IsSameCombination(string[] combination)
        {
            if (_combination.Length != combination.Length)
            {
                return false;
            }
            var combinationList = combination.ToList();
            foreach (var rune in _combination)
            {
                if (!combinationList.Contains(rune))
                {
                    return false;
                }
                combinationList.Remove(rune);
            }
            return combinationList.Count == 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MagicSkills
{
    public static class MagicSpellStatic
    {
        private static Dictionary<SpellCombination, string> _allSpellsDictionary = SpellsCombinationAndName();
        private static List<IMagicSpell> MagicSpellsList = AllSpells();
        //public static IMagicSpell[] MagicSpells = MagicSpellsList.ToArray();
        public static List<IMagicSpell> MagicSpellsSortByElements = AllSpellsListSortByElements();
        public static List<IMagicSpell> MagicSpellsSortByForm = AllSpellsListSortByForm();
        

        public static List<string> GetCharacterFaceEffectsNames()
        {
            IEnumerable<Type> types = Assembly.GetAssembly(typeof(IMagicSpell)).GetTypes().Where(findType => typeof(IMagicSpell).IsAssignableFrom(findType) && !findType.IsAbstract);
            List<string> result = new List<string>();


            foreach (var t in types)
            {
                result.Add(t.Name);
            }
            return result;
        }

        public static IMagicSpell GetSpellWithName(string skillName)
        {
            IEnumerable<Type> types = Assembly.GetAssembly(typeof(IMagicSpell)).GetTypes().Where(findType => typeof(IMagicSpell).IsAssignableFrom(findType) && !findType.IsAbstract);
            
            foreach (var t in types)
            {
                if (t.Name == skillName)
                {
                    return (IMagicSpell)Activator.CreateInstance(t);
                }
            }
            Debug.LogError($"No IMagicSpell with name {skillName}");
            return null;
            throw new System.NotImplementedException();
        }

        private static List<IMagicSpell> AllSpells()
        {
            IEnumerable<Type> types = Assembly.GetAssembly(typeof(IMagicSpell)).GetTypes().Where(findType => typeof(IMagicSpell).IsAssignableFrom(findType) && !findType.IsAbstract);

            List<IMagicSpell> spellsList = new List<IMagicSpell>();

            foreach (var t in types)
            {
                spellsList.Add((IMagicSpell)Activator.CreateInstance(t));
            }
            return spellsList;
        }

        private static List<IMagicSpell> AllSpellsListSortByElements()
        {
            List<IMagicSpell> resultList = new List<IMagicSpell>();
            foreach (var elementRune in Enum.GetNames(typeof(BasicElementsRune)))
            {
                var allSpellsWithElement = MagicSpellsList.FindAll(spell =>
                {
                    foreach (var element in spell.GetCombination())
                    {
                        if (element == elementRune)
                        {
                            return true;
                        }
                    }
                    return false;
                });
                
                allSpellsWithElement.Sort(SomeStatic.CompareSpellsByRunesCount);
                
                resultList.AddRange(allSpellsWithElement);
            }

            foreach (var formRune in Enum.GetNames(typeof(FormRune)))
            {
                var spell = MagicSpellsList.Find(spell =>
                {
                    var combination = spell.GetCombination();
                    return combination.Length == 1 && combination[0] == formRune;
                });
                if (spell != null)
                {
                    resultList.Add(spell);
                }
            }
            return resultList;
        }
        
        private static List<IMagicSpell> AllSpellsListSortByForm()
        {
            List<IMagicSpell> resultList = new List<IMagicSpell>();
            foreach (var elementRune in Enum.GetNames(typeof(FormRune)))
            {
                var allSpellsWithElement = MagicSpellsList.FindAll(spell =>
                {
                    foreach (var element in spell.GetCombination())
                    {
                        if (element == elementRune)
                        {
                            return true;
                        }
                    }
                    return false;
                });
                
                allSpellsWithElement.Sort(SomeStatic.CompareSpellsByRunesCount);
                
                resultList.AddRange(allSpellsWithElement);

            }
            return resultList;
        }


        private static Dictionary<SpellCombination, string> SpellsCombinationAndName()
        {
            IEnumerable<Type> types = Assembly.GetAssembly(typeof(IMagicSpell)).GetTypes().Where(findType => typeof(IMagicSpell).IsAssignableFrom(findType) && !findType.IsAbstract);
            Dictionary<SpellCombination, string> allSpells = new Dictionary<SpellCombination, string>();

            foreach (var t in types)
            {
                var iMagicSpell = (IMagicSpell)Activator.CreateInstance(t);
                allSpells.Add(new SpellCombination(iMagicSpell.GetCombination()), t.Name);
            }

            return allSpells;
        }

        public static IMagicSpell GetSpellWithCombination(string[] combination)
        {
            foreach (var spell in _allSpellsDictionary)
            {
                if (spell.Key.IsSameCombination(combination))
                {
                    return GetSpellWithName(spell.Value);
                }
            }
            return null;
        }

        public static IMagicSpell GetSpellNameWithCombination(string[] combination)
        {
            return (from spellPair in _allSpellsDictionary where spellPair.Key.IsSameCombination(combination) select MagicSpellsList.Find(spell => spell.GetType().Name == spellPair.Value)).FirstOrDefault();
        }
        
        public static List<IMagicSpell> GetAllSpellsWithRune(string rune)
        {
            List<IMagicSpell> resultList = MagicSpellsList.Where(spell => spell.GetCombination().Contains(rune)).ToList();
            resultList.Sort(SomeStatic.CompareSpellsByRunesCount);
            return resultList;
        }
    }
}
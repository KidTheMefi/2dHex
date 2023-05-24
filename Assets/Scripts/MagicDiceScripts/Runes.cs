
using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using ScriptableScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FormRune
{
   Arrow, Blade, Shield, Summon, Flow, Sphere
}

public enum BasicElementsRune
{
   Fire, Water, Wind, Earth, Life, Mana
}

public static class Runes
{
   public static readonly List<RuneScriptable> _runeScriptableList = RuneScriptableList();
   public static RuneScriptable EmptyRune => Resources.Load<RuneScriptable>("ScriptableObjects/Empty");
   
   public static List<string> GetRunesNamesList()
   {
      List<string> stringList = new List<string>();
      stringList.AddRange(Enum.GetNames(typeof(BasicElementsRune)));
      stringList.AddRange(Enum.GetNames(typeof(FormRune)));
      return stringList;
   }

   static List<RuneScriptable> RuneScriptableList()
   {
      var runeScriptableList = new List<RuneScriptable>();
      foreach (var rune in Resources.LoadAll<RuneScriptable>("ScriptableObjects/BaseElementsRunes"))
      {
         runeScriptableList.Add(rune);
      }

      foreach (var rune in Resources.LoadAll<RuneScriptable>("ScriptableObjects/FormRunes"))
      {
         runeScriptableList.Add(rune);
      }

      /*var empty = runeScriptableList.Find(rune => rune.RuneName == "Empty");
      runeScriptableList.Remove(empty);
      runeScriptableList.Insert(0,empty);*/
      
      
      return runeScriptableList;
   }
   
   public static RuneScriptable GetRuneScriptable(string runeName)
   {
      return _runeScriptableList.FirstOrDefault(rune => rune.RuneName == runeName);
   }
   
   public static RuneScriptable GetRandomRuneScriptable()
   {
      var allRunes = GetRunesNamesList();
      string runeName = allRunes[Random.Range(0, allRunes.Count)];
      return _runeScriptableList.FirstOrDefault(rune => rune.RuneName == runeName);
   }
}

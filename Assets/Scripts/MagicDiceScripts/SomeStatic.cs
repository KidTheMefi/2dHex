using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicSkills;
using UnityEditor;
using UnityEngine;
using Random = System.Random;



public class SomeStatic
{
    static Random _R = new Random();
    public static T RandomEnumValue<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(_R.Next(v.Length));
    }

    public static string AddSpacesToSentence(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }

    public static Queue<T> ListToQueue<T>(List<T> list)
    {
        Queue<T> resultQueue = new Queue<T>();
        foreach (var variable in list)
        {
            resultQueue.Enqueue(variable);
        }
        return resultQueue;
    }

    public static IEnumerable<T[]> Combinations<T>(IEnumerable<T> source)
    {
        if (null == source)
            throw new ArgumentNullException(nameof(source));

        T[] data = source.ToArray();

        return Enumerable
            .Range(0, 1 << (data.Length))
            .Select(index => data
                .Where((v, i) => (index & (1 << i)) != 0)
                .ToArray());
    }

    public static int CompareSpellsByRunesCount(IMagicSpell firstSpell, IMagicSpell secondSpell)
    {
        if (firstSpell == null)
        {
            if (secondSpell == null)
            {
                // If x is null and y is null, they're
                // equal.
                return 0;
            }
            else
            {
                // If x is null and y is not null, y
                // is greater.
                return -1;
            }
        }
        else
        {
            // If x is not null...
            //
            if (secondSpell == null)

                // ...and y is null, x is greater.
            {
                return 1;
            }
            else
            {
                // ...and y is not null, compare the
                // lengths of the two strings.
                //

                int retval = firstSpell.GetCombination().Length > secondSpell.GetCombination().Length ? 1 : -1;

                return retval;
                /*if (retval != 0)
                {
                    // If the strings are not of equal length,
                    // the longer string is greater.
                    //
                    return retval;
                }
                else
                {
                    // If the strings are of equal length,
                    // sort them with ordinary string comparison.
                    //
                    return x.CompareTo(y);*/
            }
        }
    }

}
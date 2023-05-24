using System.Collections.Generic;
using UnityEngine;

namespace BattleFieldScripts
{
    public enum PossibleTarget
    {
        AnyPosition,
        AnyPositionWithCharacter,
        FirstInRow,
        FirstColumnWithCharacter,
        AnyPositionWithoutCharacter
    }

    public class TargetingPossibility
    {
        private TeamField _teamField;

        public TargetingPossibility(TeamField teamField)
        {
            _teamField = teamField;
        }
        public List<Vector2Int> GetPossibleTargets(PossibleTarget possibleTarget)
        {
            return possibleTarget switch
            {
                PossibleTarget.AnyPosition => AnyPositionTarget(),
                PossibleTarget.FirstInRow => FirstInRow(),
                PossibleTarget.AnyPositionWithCharacter => AllPositionWithCharacter(),
                PossibleTarget.AnyPositionWithoutCharacter => AllPositionWithoutCharacter(),
                PossibleTarget.FirstColumnWithCharacter => FirstColumnWithCharacter(),
                _ => AnyPositionTarget()
            };
        }

        public List<Vector2Int> EveryEnemyInLine(int lineCount)
        {
            List<Vector2Int> result = new List<Vector2Int>();
            if (!(lineCount < _teamField.PositionsVector.Length))
            {
                return result;
            }
            
            for (int i = 0; i < _teamField.PositionsVector[lineCount].Length; i++)
            {
                if (_teamField.PositionDictionary.TryGetValue(_teamField.PositionsVector[i][lineCount], out var pos) && !pos.IsEmpty())
                {
                    result.Add(new Vector2Int(i, lineCount));
                }
            }
            return result;
        }
        

    public List<Vector2Int> FirstColumnWithCharacters()
    {
        return FirstColumnWithCharacter();
    }

    private List<Vector2Int> AnyPositionTarget()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (var position in _teamField.PositionDictionary)
        {
            result.Add(position.Key);
        }
        return result;
    }

    private List<Vector2Int> AllPositionWithCharacter()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (var position in _teamField.PositionDictionary)
        {
            if (!position.Value.IsEmpty())
            {
                result.Add(position.Key);
            }
        }
        return result;
    }

    private List<Vector2Int> FirstInRow()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        for (int i = 0; i < _teamField.PositionsVector.Length; i++)
        {
            for (int j = 0; j < _teamField.PositionsVector[i].Length; j++)
            {
                if (_teamField.PositionDictionary.TryGetValue(_teamField.PositionsVector[j][i], out var pos) && !pos.IsEmpty())
                {
                    result.Add(new Vector2Int(j, i));
                    break;
                }
            }
        }
        return result;
    }

    private List<Vector2Int> FirstColumnWithCharacter()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = 0; i < _teamField.PositionsVector.Length; i++)
        {
            bool columnHasEnemy = false;
            for (int j = 0; j < _teamField.PositionsVector[i].Length; j++)
            {
                if (_teamField.PositionDictionary.TryGetValue(_teamField.PositionsVector[i][j], out var pos) && !pos.IsEmpty())
                {
                    result.Add(new Vector2Int(i, j));
                    columnHasEnemy = true;
                }
            }
            if (columnHasEnemy)
            {
                break;
            }
        }
        return result;
    }

    private List<Vector2Int> AllPositionWithoutCharacter()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        foreach (var position in _teamField.PositionDictionary)
        {
            if (position.Value.IsEmpty())
            {
                result.Add(position.Key);
            }
        }
        return result;
    }
}

}
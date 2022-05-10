using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Interfaces;
using PlayerGroup;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerPathFind
{
    private IHexStorage _hexStorage;
    private AStarSearch _pathFind;
    private PlayerGroupModel _playerGroupModel;
    private PathPoint.Factory _pathPointFactory;

    private Dictionary<Hex, PathPoint> _pathPointsAtCoordinates = new Dictionary<Hex, PathPoint>();

    public PlayerPathFind(IHexStorage hexStorage, AStarSearch pathFind, PlayerGroupModel playerGroupModel, PathPoint.Factory pathPointFactory)
    {
        _hexStorage = hexStorage;
        _pathFind = pathFind;
        _playerGroupModel = playerGroupModel;
        _pathPointFactory = pathPointFactory;
    }
    public void ClearPath()
    {
        foreach (var point in _pathPointsAtCoordinates)
        {
            point.Value.Dispose();
        }
        _pathPointsAtCoordinates.Clear();
    }

    public void RemovePoint(Hex hex)
    {
        _pathPointsAtCoordinates[hex].Dispose();
        _pathPointsAtCoordinates.Remove(hex);
    }
    
    public Hex[] GetPath()
    {
        return _pathPointsAtCoordinates.Keys.ToArray();
    }

    public async UniTask PathFindTest(Vector2Int target) // maybe TODO: smth with that
    {
        var starPathPos = _playerGroupModel.AxialPosition;
        var endPathPos = target;
        int energyCost = 0;
        int timeCost = 0;
        var pathList = await _pathFind.TryPathFind(starPathPos, endPathPos);
        if (pathList.Count != 0)
        {
            ClearPath();
            pathList.Insert(0, _hexStorage.GetHexAtAxialCoordinate(endPathPos));
            pathList.Reverse();

            foreach (var pathCoordinate in pathList)
            {
                if (pathCoordinate.AxialCoordinate != starPathPos)
                {
                    energyCost += pathCoordinate.LandTypeProperty.MovementEnergyCost;
                    timeCost += pathCoordinate.LandTypeProperty.MovementTimeCost;

                    if (energyCost > _playerGroupModel.Energy)
                    {
                        
                        break;
                    }
                    
                    var point = _pathPointFactory.Create();
                    var text = String.Format("{0}|{1}", pathCoordinate.LandTypeProperty.MovementEnergyCost.ToString(), pathCoordinate.LandTypeProperty.MovementTimeCost.ToString());
                    point.SetPathPoint(pathCoordinate.Position, text);
                    _pathPointsAtCoordinates.Add(pathCoordinate, point);
                   
                }
            }
            //Debug.Log(energyCost);
            Debug.Log(String.Format("Energy cost: {0} // Time cost:{1}", energyCost, timeCost));
        }
        else
        {
            ClearPath();
        }
        await UniTask.Yield();
    }
}
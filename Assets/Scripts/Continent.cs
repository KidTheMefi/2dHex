using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Continent
{
    private IHexStorage _hexStorage;
    private ILandGeneration _landGeneration;

    private List<Vector2Int> _continentAllHex;
    private Dictionary<LandType, List<Vector2Int>> _landTypes;
    
    public Dictionary<LandType, List<Vector2Int>> LandTypes => _landTypes;
    public List<Vector2Int> AllHexes => _continentAllHex;

    public Continent(IHexStorage hexStorage, ILandGeneration landGeneration)
    {
        _hexStorage = hexStorage;
        _landGeneration = landGeneration;
    }
    
    public void RemoveFromContinent(Vector2Int hexPos)
    {
        foreach (var landType in _landTypes)
        {
            landType.Value.Remove(hexPos);
        }
        _continentAllHex.Remove(hexPos);
    }

    public async UniTask CreateContinent(Vector2Int continentPos, ContinentSettings continentSettings, List<Vector2Int> unavailableHexes = null)
    {
        _landTypes = new Dictionary<LandType, List<Vector2Int>>();

        _continentAllHex = _landGeneration.CreateContinentLand(continentPos, 2, continentSettings.HexCount, unavailableHexes);
        var continentFreeLand = new List<Vector2Int>();

        foreach (var hex in _continentAllHex)
        {
            continentFreeLand.Add(hex);
        }
        
        Vector2Int centerAxial = CenterOf(_continentAllHex);

        foreach (var lands in continentSettings.Lands)
        {
            CreateLandTypeAt(lands.LandType, continentFreeLand, lands.Percent);
        }
        
        CreateLandTypeAt(continentSettings.DefaultLandType, continentFreeLand);
        await UniTask.Yield();
    }

    private void CreateLandTypeAt(LandType landType, List<Vector2Int> availableHexes, int percent)
    {
        var landList = _landGeneration.CreateLandTypeAtContinent(availableHexes, _continentAllHex.Count * percent / 100);
        
        foreach (var axial in landList)
        {
            _hexStorage.GetHexAtAxialCoordinate(axial).SetLandType(landType);
        }
        _landTypes.Add(landType, landList);
    }
    
    private void CreateLandTypeAt(LandType landType, List<Vector2Int> availableHexes)
    {
        foreach (var hex in availableHexes)
        {
            _hexStorage.GetHexAtAxialCoordinate(hex).SetLandType(landType);
        }
        _landTypes.Add(landType, availableHexes);
    }
    
    private Vector2Int CenterOf(List<Vector2Int> continent)
    {
        Vector2Int centerSum = new Vector2Int(0, 0);

        foreach (var vector2 in continent)
        {
            centerSum += vector2;
        }
        return new Vector2Int(centerSum.x / continent.Count, centerSum.y / continent.Count);
    }
}
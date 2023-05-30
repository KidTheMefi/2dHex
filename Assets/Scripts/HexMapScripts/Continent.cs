using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[Serializable]
public class Continent
{
    private IHexStorage _hexStorage;
    private ILandGeneration _landGeneration;

    [SerializeField]
    private List<Vector2Int> _continentAllHex;
    [SerializeField]
    private BiomType _biomType;
    [SerializeField]
    private List<TilesWithSameLandType> _tilesWithSameLandType;
    public List<TilesWithSameLandType> TilesWithSameLandTypeList => _tilesWithSameLandType;
    
    
    public List<Vector2Int> AllHexes => _continentAllHex;
    public BiomType BiomType => _biomType;

    public Continent(IHexStorage hexStorage, ILandGeneration landGeneration)
    {
        _hexStorage = hexStorage;
        _landGeneration = landGeneration;
    }

    public void RemoveFromContinent(Vector2Int hexPos)
    {
        foreach (var landType in _tilesWithSameLandType)
        {
            landType.tilesAxialPositions.Remove(hexPos);
        }
        _continentAllHex.Remove(hexPos);
    }
    
    public class Factory : PlaceholderFactory<Continent>
    {
        
    }

    public async UniTask CreateNewContinent(Vector2Int continentPos, ContinentSettings continentSettings, int tilesCount, List<Vector2Int> unavailableHexes = null)
    {
        _biomType = continentSettings.BiomType;
        _tilesWithSameLandType = new List<TilesWithSameLandType>();

        _continentAllHex = _landGeneration.CreateContinentLand(continentPos, 5, tilesCount, unavailableHexes);

        if (_continentAllHex.Count < tilesCount)
        {
            Debug.LogError(String.Format("To many cycles in LandCreating. Only {0}/{1} tiles produces", _continentAllHex.Count, tilesCount ));
        }
        
        var continentFreeLand = new List<Vector2Int>();
        continentFreeLand.AddRange(_continentAllHex);

        foreach (var lands in continentSettings.Lands)
        {
            CreateLandTypeAt(lands.LandType, continentFreeLand, lands.Percent);
        }

        CreateLandTypeAt(continentSettings.DefaultLandType, continentFreeLand);
        await UniTask.Yield();
    }

    private void CreateLandTypeAt(LandTypeProperty landType, List<Vector2Int> availableHexes, int percent)
    {
        var landList = _landGeneration.CreateLandTypeAtContinent(availableHexes, _continentAllHex.Count * percent / 100);

        foreach (var axial in landList)
        {
            _hexStorage.GetHexAtAxialCoordinate(axial).SetLandTypeProperty(landType);
        }
        _tilesWithSameLandType.Add(new TilesWithSameLandType(landType.LandType, landList));
    }

    private void CreateLandTypeAt(LandTypeProperty landType, List<Vector2Int> availableHexes)
    {
        foreach (var hex in availableHexes)
        {
            _hexStorage.GetHexAtAxialCoordinate(hex).SetLandTypeProperty(landType);
        }
        _tilesWithSameLandType.Add(new TilesWithSameLandType(landType.LandType, availableHexes));
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

    public Vector2Int GetRandomHexAtContinent()
    {
        return _continentAllHex[Random.Range(0, _continentAllHex.Count)];
    }
    
    [Serializable]
    public struct TilesWithSameLandType
    {
        public LandType landType { get; private set; }
        public List<Vector2Int> tilesAxialPositions { get; private set; }

        public TilesWithSameLandType(LandType landType, List<Vector2Int> tilesAxialPositions)
        {
            this.landType = landType;
            this.tilesAxialPositions = tilesAxialPositions;
        }
    }
}
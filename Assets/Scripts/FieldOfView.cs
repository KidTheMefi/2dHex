using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;
using PlayerGroup;
using UnityEngine;
using Zenject;

public class FieldOfView : IInitializable
{
    private IHexStorage _hexStorage;
    private PlayerGroupModel _playerGroupModel;

    private List<Hex> _currentOpenedHexes = new List<Hex>();
    public FieldOfView(IHexStorage hexStorage, PlayerGroupModel playerGroupModel)
    {
        _hexStorage = hexStorage;
        _playerGroupModel = playerGroupModel;
    }


    public void Initialize()
    {
        //_playerGroupModel.PositionChanged += UpdateFieldOfView;
        _playerGroupModel.PositionChanged += (center, radius) => UpdateFieldOfView(center, radius).Forget();
    }

    private async UniTask UpdateFieldOfView(Vector2Int center, int radius)
    {
        int radiusActual = radius;
        switch (_hexStorage.GetHexAtAxialCoordinate(center).LandTypeProperty.LandType)
        {
            case LandType.Forrest:
                if (radiusActual > 1)
                {
                    radiusActual--;
                }
                break;
            case LandType.Hill:
                radiusActual++;
                break;
            default:
                break;
        }

        List<Hex> hexThatCanBeSee = new List<Hex>();
        var ring = HexUtils.GetAxialRingWithRadius(center, radiusActual);

        foreach (var hexInRing in ring)
        {
            var line = HexUtils.GetAxialLine(center, hexInRing);

            foreach (var hexCoordinate in line)
            {
                if (_hexStorage.HexAtAxialCoordinateExist(hexCoordinate))
                {
                    var hex = _hexStorage.GetHexAtAxialCoordinate(hexCoordinate);

                    if (!hexThatCanBeSee.Contains(hex))
                    {
                        hexThatCanBeSee.Add(hex);
                        _currentOpenedHexes.Remove(hex);
                    }

                    _hexStorage.HexToHexView()[hex].SetHexVisible(false);
                    if (hex.LandTypeProperty.LandType == LandType.Mountain)
                    {
                        break;
                    }

                }
            }
        }

        foreach (var hex in _currentOpenedHexes)
        {
            _hexStorage.HexToHexView()[hex].SetHexVisible(true);
        }

        _currentOpenedHexes = hexThatCanBeSee;

        await UniTask.Yield();
    }
}
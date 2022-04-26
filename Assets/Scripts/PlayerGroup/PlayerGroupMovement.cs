using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Zenject;

public class PlayerGroupMovement : IInitializable
{
    private MapGeneration _mapGeneration;
    private IRandomPassablePosition _randomPassablePosition;
    private PlayerGroupModel _playerGroupModel;
    private PlayerGroupView _playerGroupView;
    
    public PlayerGroupMovement(MapGeneration mapGeneration, IRandomPassablePosition randomPassablePosition, PlayerGroupModel playerGroupModel, PlayerGroupView playerGroupView)
    {
        _mapGeneration = mapGeneration;
        _randomPassablePosition = randomPassablePosition;
        _playerGroupModel = playerGroupModel;
        _playerGroupView = playerGroupView;
    }
    
    public void Initialize()
    {
        _mapGeneration.MapGenerated += SpawnAtRandomPosition;
    }

    private void SpawnAtRandomPosition()
    {
        var axialPos = _randomPassablePosition.GetRandomStartPosition();
        _playerGroupModel.SetAxialPosition(axialPos);
        _playerGroupView.transform.position = HexUtils.CalculatePosition(axialPos);
    }


}

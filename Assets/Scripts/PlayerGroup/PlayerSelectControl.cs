using System;
using System.Collections;
using System.Collections.Generic;
using PlayerGroup;
using UnityEngine;
using Zenject;

public class PlayerSelectControl : IInitializable, IDisposable
{
    private PlayerGroupModel _playerGroupModel;
    private PlayerGroupView _playerGroupView;
    private HexMouseController _hexMouseController;
    
    public PlayerSelectControl(PlayerGroupModel playerGroupModel, PlayerGroupView playerGroupView, HexMouseController hexMouseController)
    {
        _playerGroupModel = playerGroupModel;
        _playerGroupView = playerGroupView;
        _hexMouseController = hexMouseController;
    }
    public void Initialize()
    {
        _hexMouseController.HighlightedHexClicked += Selected;
    }

    private void Selected(Vector2Int axial)
    {
        if ( axial == _playerGroupModel.AxialPosition && !_playerGroupModel.IsSelected )
        {
            _playerGroupModel.Select(true);
            _playerGroupView.ShowSelect(true);
        }
        else if (axial != _playerGroupModel.AxialPosition)
        {
            _playerGroupModel.Select(false);
            _playerGroupView.ShowSelect(false);
        }
    }
    
    public void Dispose()
    {
        _hexMouseController.HighlightedHexClicked -= Selected;
    }
}

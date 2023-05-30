using System;
using UnityEngine;


public enum TileVisibility
{
    Undiscovered, Discovered, VisibleNow, VisibleAlways
}

[Serializable]
public class Hex 
{
    public event Action<Hex, Sprite> LandTypeSpriteChanged = delegate { };
    public event Action<Hex, TileVisibility> VisibilityChanged = delegate(Hex hex, TileVisibility visibility) { };
    
    [SerializeField]
    public Vector2Int AxialCoordinate;
    [SerializeField]
    public Vector3 Position;
    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite => _sprite;
    [SerializeField]
    private TileVisibility _tileVisibility;
    public TileVisibility TileVisibility => _tileVisibility;
    
    [SerializeField]
    private LandTypeProperty _landTypeProperty;
    public LandTypeProperty LandTypeProperty => _landTypeProperty;

    public void SetLandTypeProperty(LandTypeProperty landTypeProperty)
    {
        _landTypeProperty = landTypeProperty;
        _sprite = _landTypeProperty.GetSprite();
        LandTypeSpriteChanged.Invoke(this, _landTypeProperty.GetSprite());
    }

    public void SetVisibility(TileVisibility visibility)
    {
        _tileVisibility = visibility;
        VisibilityChanged.Invoke(this, _tileVisibility);
    }

    public Hex(Vector2Int axialCoordinate)
    {
        AxialCoordinate = axialCoordinate;
        Position = HexUtils.CalculatePosition(axialCoordinate);
    }

    public void LoadHex(Hex hex)
    {
        AxialCoordinate = hex.AxialCoordinate;
        Position = hex.Position;
        _sprite = hex._sprite;
        _tileVisibility = hex._tileVisibility;
        _landTypeProperty = hex._landTypeProperty;
    }

    public void InvokeLandTypeSpriteChanged()
    {
        LandTypeSpriteChanged.Invoke(this, _landTypeProperty.GetSprite());
    }

}

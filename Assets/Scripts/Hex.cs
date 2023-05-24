using System;
using UnityEngine;

[Serializable]
public class Hex 
{
    public event Action<Hex, Sprite> LandTypeSpriteChanged = delegate { };

    [SerializeField]
    public Vector2Int AxialCoordinate;
    [SerializeField]
    public Vector3 Position;
    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite => _sprite;
    
    [SerializeField]
    private LandTypeProperty _landTypeProperty;
    public LandTypeProperty LandTypeProperty => _landTypeProperty;

    public void SetLandTypeProperty(LandTypeProperty landTypeProperty)
    {
        _landTypeProperty = landTypeProperty;
        _sprite = _landTypeProperty.GetSprite();
        LandTypeSpriteChanged.Invoke(this, _landTypeProperty.GetSprite());
    }

    public Hex(Vector2Int axialCoordinate)
    {
        AxialCoordinate = axialCoordinate;
        Position = HexUtils.CalculatePosition(axialCoordinate);
    }

    public void InvokeLandTypeSpriteChanged()
    {
        LandTypeSpriteChanged.Invoke(this, _landTypeProperty.GetSprite());
    }

}

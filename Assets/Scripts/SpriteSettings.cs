using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum LandType
{
    Water, Grass, Hill, Mountain, Desert, Snow, Forrest, SnowMountain, SnowForrest, SnowHill
}

[CreateAssetMenu(menuName = "HexGame/SpriteSettings")]
public class SpriteSettings : ScriptableObject
{
    [SerializeField] private List<HexLand> Sprites;

    public Sprite GetSprite(LandType landType)
    {
        HexLand land = Sprites.Find(s => s.LandType == landType);
        Assert.IsNotNull(land, "There is no " + landType + "in SpriteSetting");
        return land.Sprites[Random.Range(0, land.Sprites.Count)];
    }
}

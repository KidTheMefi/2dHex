using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum TerrainType
{
    Water, Grass, Hill, Mountain, Desert, Snow, Forrest
}

[CreateAssetMenu(menuName = "HexGame/SpriteSettings")]
public class SpriteSettings : ScriptableObject
{
    [SerializeField] private List<HexLand> Sprites;

    public Sprite GetSprite(TerrainType terrainType)
    {
        HexLand land = Sprites.Find(s => s.TerrainType == terrainType);
        Assert.IsNotNull(land, "There is no " + terrainType + "in SpriteSetting");
        return land.Sprites[Random.Range(0, land.Sprites.Count)];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum LandType
{
   Water, Plain, Forrest, Hill, Mountain
}

[CreateAssetMenu(menuName = "HexGame/LandTypeSettings")]
public class LandTypeProperty : ScriptableObject
{
   
   [SerializeField] private string landName;
   [SerializeField] private LandType _landType;
   [SerializeField] private bool _isPassable = true;
   [SerializeField] private int _movementTimeCost = 1;
   [SerializeField] private int _movementEnergyCost = 1;
   [SerializeField] private List<Sprite> _landTypeSprites;

   public bool IsPassable => _isPassable;
   public string LandName => landName;
   public int MovementTimeCost => _movementTimeCost;
   public int MovementEnergyCost => _movementEnergyCost;
   public LandType LandType => _landType;

   public Sprite GetSprite()
   {
      Assert.IsNotNull(_landTypeSprites, "There are no sprites in LandTypeProperty " + landName);
      return _landTypeSprites[Random.Range(0, _landTypeSprites.Count)];
   }
}

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
   [SerializeField] private bool _isPassible = true;
   [SerializeField] private int _movementCost = 1;
   [SerializeField] private List<Sprite> _landTypeSprites;

   public bool IsPassible => _isPassible;
   public string LandName => landName;
   public int MovementCost => _movementCost;
   public LandType LandType => _landType;
   
   public Sprite GetSprite()
   {
      Assert.IsNotNull(_landTypeSprites, "There are no sprites in LandTypeProperty " + landName);
      return _landTypeSprites[Random.Range(0, _landTypeSprites.Count)];
   }
}

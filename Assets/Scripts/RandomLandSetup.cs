using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "HexGame/RandomLandSetup")]
public class RandomLandSetup : ScriptableObject
{
    [SerializeField] private int _ocean;
    [SerializeField] private int _grass;
    [SerializeField] private int _hill;
    [SerializeField] private int _mountain;
    
    public void OnValidate()
    {
        //throw new NotImplementedException();
    }
}

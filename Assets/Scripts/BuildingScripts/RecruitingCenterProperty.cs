using System;
using System.Collections.Generic;
using ScriptableScripts;
using UnityEngine;

namespace BuildingScripts
{
    [CreateAssetMenu(menuName = "Buildings/RecruitingCenter")]
    public class RecruitingCenterProperty : ScriptableObject
    {
        public Sprite sprite;
        public string name;
        public string description;
        public LandType landType;
        public BiomType landBiom;
        public List<CharacterScriptable> Recruits;
    }

    
}
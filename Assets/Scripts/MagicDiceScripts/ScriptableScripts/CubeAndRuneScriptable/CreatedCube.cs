using System.Collections.Generic;
using UnityEngine;

namespace ScriptableScripts.CubeAndRuneScriptable
{
    [CreateAssetMenu(menuName = "Cubes/CreatedCube", fileName = "CreatedCube")]
    public class CreatedCube : CubeSetup
    {
        private List<RuneScriptable> _runes = new List<RuneScriptable>();
        
        public void CleanAllRunes()
        {
            _runes = new List<RuneScriptable>();

        }
        
        public void SaveRunes(List<RuneScriptable> runes)
        {
            _runes = runes;
        }
        
        public override RuneScriptable[] GetCubeRuneScriptable()
        {
            RuneScriptable[] cubeRunes = new RuneScriptable[6];
            for (int i = 0; i < cubeRunes.Length; i++)
            {
                if (i < _runes.Count)
                {
                    
                    cubeRunes[i] = _runes[i];
                }
                else
                {
                    cubeRunes[i] = null;
                }
            }
            return cubeRunes;
        }
    }
}
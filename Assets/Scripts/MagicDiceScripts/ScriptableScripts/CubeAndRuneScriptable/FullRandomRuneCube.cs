using UnityEngine;

namespace ScriptableScripts
{
    [CreateAssetMenu(menuName = "Cubes/RandomCube", fileName = "RandomCube")]
    public class FullRandomRuneCube  : CubeSetup
    {
        public override RuneScriptable[] GetCubeRuneScriptable()
        {
            RuneScriptable[] cubeRunes = new RuneScriptable[6];

            for (int i = 0; i < cubeRunes.Length; i++)
            {
                cubeRunes[i] = Runes.GetRandomRuneScriptable();
            }
            return cubeRunes;
        }
    }
}
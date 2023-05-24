using DefaultNamespace;
using UnityEngine;

namespace ScriptableScripts
{
    [CreateAssetMenu(menuName = "Cubes/BasicElementsCube", fileName = "Basic Elements Cube")]
    public class BaseElementsCube : CubeSetup
    {
        [SerializeField]
        private BasicElementsRune[] _basicElementsRunes = new BasicElementsRune[6];

        public override RuneScriptable[] GetCubeRuneScriptable()
        {
            RuneScriptable[] cubeRunes = new RuneScriptable[6];

            for (int i = 0; i < cubeRunes.Length; i++)
            {
                if (i < _basicElementsRunes.Length)
                {
                    cubeRunes[i] = Runes.GetRuneScriptable(_basicElementsRunes[i].ToString());
                }
                else
                {
                    cubeRunes[i] = null;
                }
            }
            return cubeRunes;
        }
        private void OnValidate()
        {
            if (_basicElementsRunes.Length <= 6) return;
            BasicElementsRune[] newElementsRunes = new BasicElementsRune[6];
            for (int i = 0; i < newElementsRunes.Length; i++)
            {
                newElementsRunes[i] = _basicElementsRunes[i];
            }
            _basicElementsRunes = newElementsRunes;
        }
    }
}
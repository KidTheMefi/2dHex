 using UnityEngine;

namespace ScriptableScripts.CubeAndRuneScriptable
{
    [CreateAssetMenu(menuName = "Cubes/AllRuneCube", fileName = "Cube")]
    public class AllRuneCube : CubeSetup
    {
        [SerializeField]
        private AnyRuneString[] _runes = new AnyRuneString[6];

        public override RuneScriptable[] GetCubeRuneScriptable()
        {
            RuneScriptable[] cubeRunes = new RuneScriptable[6];

            for (int i = 0; i < cubeRunes.Length; i++)
            {
                if (i < _runes.Length)
                {
                    cubeRunes[i] = Runes.GetRuneScriptable(_runes[i].RuneName);
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
            if (_runes.Length <= 6) return;
            AnyRuneString[] newElementsRunes = new AnyRuneString[6];
            for (int i = 0; i < newElementsRunes.Length; i++)
            {
                newElementsRunes[i] = _runes[i];
            }
            _runes = newElementsRunes;
        }
    }
}
using UnityEngine;

namespace ScriptableScripts
{
    [CreateAssetMenu(menuName = "Cubes/FormRunesCube", fileName = "Form Runes Cube")]
    public class FormsCube: CubeSetup
    {
        [SerializeField]
        private FormRune[] _basicElementsRunes = new FormRune[6];

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
            FormRune[] newElementsRunes = new FormRune[6];
            for (int i = 0; i < newElementsRunes.Length; i++)
            {
                newElementsRunes[i] = _basicElementsRunes[i];
            }
            _basicElementsRunes = newElementsRunes;
        }
    }
}
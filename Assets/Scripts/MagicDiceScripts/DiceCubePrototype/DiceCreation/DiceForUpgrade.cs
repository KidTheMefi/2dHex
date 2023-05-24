using System.Collections.Generic;
using System.Linq;
using ScriptableScripts.CubeAndRuneScriptable;
using UnityEngine;

namespace DiceCubePrototype
{
    public class DiceForUpgrade : MonoBehaviour
    {
        private CreatedCube _scriptableCubeSetting;
        public CreatedCube ScriptableCubeSetting => _scriptableCubeSetting;
        [SerializeField]
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody;
        private List<DiceFaceInCreation> _cubeDiceFaces;
        public List<DiceFaceInCreation> CubeDiceFaces => _cubeDiceFaces;

        public void SetScriptableSettings(CreatedCube scriptableCubeSetting)
        {
            _scriptableCubeSetting = scriptableCubeSetting;
            
            var runes = _scriptableCubeSetting.GetCubeRuneScriptable();
            for (int i = 0; i < runes.Length; i++)
            {
                if (i < _cubeDiceFaces.Count)
                {
                    _cubeDiceFaces[i].Selected(false);
                    //Debug.Log(runes[i].RuneName);
                    _cubeDiceFaces[i].SetRune(runes[i]);
                }
            }
            SetFacesClickable(false);
        }
        
        private void Awake()
        {
            _cubeDiceFaces = GetComponentsInChildren<DiceFaceInCreation>().ToList();
        }
        public void DisableInteraction()
        {
            UnSelectFaces();
            SetFacesClickable(false);
        }

        public void UnSelectFaces()
        {
            foreach (var face in _cubeDiceFaces)
            {
                face.Selected(false);
            }
        }
        public void SetFacesClickable(bool value)
        {
            foreach (var face in _cubeDiceFaces)
            {
                face.SetActiveCollision(value);
            }
        }
    }
}
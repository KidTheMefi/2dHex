using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableScripts.CubeAndRuneScriptable
{
    public enum DiceListVariant
    {
        Balanced, Created, FullRandom
    }
    
    [CreateAssetMenu(menuName = "Cubes/DiceList", fileName = "DiceList")]
    public class DiceListSetupScriptable : ScriptableObject
    {
        private DiceListVariant _diceListVariant = DiceListVariant.Balanced;
        [SerializeField]
        private List<CreatedCube> _createdCubeSetups;
        public List<CreatedCube> CreatedCubeSetups => _createdCubeSetups;
        [SerializeField]
        private BaseElementsCube _baseElementsCube;
        [SerializeField]
        private FormsCube _formsCube;
        [SerializeField]
        private FullRandomRuneCube _fullRandomCube;

        public void SetListVariant(DiceListVariant diceListVariant)
        {
            Debug.Log($"Set {diceListVariant}");
            _diceListVariant = diceListVariant;
            Debug.Log($"done {_diceListVariant}");
        }
        
        public List<CubeSetup> GetCubesSetup()
        {
            Debug.Log($"Get {_diceListVariant}");
            return _diceListVariant switch
            {
                DiceListVariant.Balanced => GetBalancedCubes(),
                DiceListVariant.FullRandom => GetRandomCubes(),
                DiceListVariant.Created => GetCreatedSetup(),
                _ => GetBalancedCubes()
            };
        }

        private List<CubeSetup> GetCreatedSetup()
        {
            return _createdCubeSetups.Cast<CubeSetup>().ToList();
        }

        private List<CubeSetup> GetBalancedCubes()
        {
            List<CubeSetup> cubeSetups = new List<CubeSetup>();
            for (int i = 0; i < 6; i++)
            {
                if (i<3)
                {
                    cubeSetups.Add(_baseElementsCube);
                }
                else
                {
                    cubeSetups.Add(_formsCube);
                }
            }
            return cubeSetups;
        }

        private List<CubeSetup> GetRandomCubes()
        {
            List<CubeSetup> cubeSetups = new List<CubeSetup>();
            for (int i = 0; i < 6; i++)
            {
                cubeSetups.Add(_fullRandomCube);
            }
            return cubeSetups;
        }
    }
}
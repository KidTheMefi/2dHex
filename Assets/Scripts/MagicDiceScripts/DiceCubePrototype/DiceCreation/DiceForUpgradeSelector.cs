using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ScriptableScripts.CubeAndRuneScriptable;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DiceCubePrototype
{
    public class DiceForUpgradeSelector : MonoBehaviour
    {
        [SerializeField]
        private DiceListSetupScriptable _diceListSetupScriptable;
        [SerializeField]
        private DiceUpgradeHandler _upgradeHandler;
        [SerializeField]
        private DiceForUpgrade _dicePrefab;
        [SerializeField]
        private DiceRotateHandler _diceRotateHandler;
        [SerializeField]
        private Transform _diceParent;
        [SerializeField]
        private Button _nextPageButton;
        [SerializeField]
        private Button _previousPageButton;
        private List<DiceForUpgrade> _dices;
        [SerializeField]
        private float _diceDistance;
        [SerializeField]
        private float _moveSpeed;
        private List<CreatedCube> _createdCubes;

        private CancellationTokenSource _movementCTS;

        private int _diceIndex = 0;

        private void Start()
        {
            _diceIndex = 0;
            _nextPageButton.onClick.AddListener(NextDice);
            _previousPageButton.onClick.AddListener(PreviousDice);

            _dices = new List<DiceForUpgrade>();
            _createdCubes = _diceListSetupScriptable.CreatedCubeSetups;
            for (int i = 0; i < _createdCubes.Count; i++)
            {
                var dice = Instantiate(_dicePrefab, _diceParent);
                dice.transform.localPosition = Vector3.zero + Vector3.right * _diceDistance * i;

                //  Quaternion.Euler(-30,50,-30)
                dice.SetScriptableSettings(_createdCubes[i]);
                _dices.Add(dice);
            }

            UpdateButtons();
            SelectDice(_dices[_diceIndex]);
            _previousPageButton.interactable = false;
        }

        private void UpdateButtons()
        {
            _nextPageButton.interactable = _diceIndex + 1 < _dices.Count;
            _previousPageButton.interactable = _diceIndex - 1 >= 0;
        }

        private void SelectDice(DiceForUpgrade diceForUpgrade)
        {
            //_upgradeHandler.StopUpgrade();
            _upgradeHandler.UpgradeDice(diceForUpgrade);
            _diceRotateHandler.SetDiceToRotate(diceForUpgrade.Rigidbody);
            diceForUpgrade.SetFacesClickable(true);
        }

        private void NextDice()
        {
            _dices[_diceIndex].DisableInteraction();
            _diceIndex++;
            ChangeDiceAsync().Forget();
        }

        private void PreviousDice()
        {
            _dices[_diceIndex].DisableInteraction();
            _diceIndex--;
            ChangeDiceAsync().Forget();
        }
        
        private async UniTask ChangeDiceAsync()
        {
            _upgradeHandler.StopUpgrade();
            UpdateButtons();
            await DiceMoveAsync();
            SelectDice(_dices[_diceIndex]);
            UpdateButtons();
        }
        private async UniTask DiceMoveAsync()
        {
            
            _movementCTS?.Cancel();
            _movementCTS = new CancellationTokenSource();
            var position = new Vector3(-_diceIndex * _diceDistance, 0, 0);
            float interpolation = 0;
            await UniTask.WaitUntil(() =>
            {
                if (_movementCTS.IsCancellationRequested)
                {
                    return true;
                }
                interpolation += _moveSpeed * Time.deltaTime;
                _diceParent.transform.position = Vector3.Lerp(_diceParent.transform.position, position, interpolation);
                return Mathf.Abs(_diceParent.transform.position.x - position.x) < 0.001;
            }, cancellationToken: _movementCTS.Token);
        }

        private void OnDestroy()
        {
            _movementCTS?.Cancel();
        }

        public void CleanAllDices()
        {
            for (int i = 0; i < _createdCubes.Count; i++)
            {
                _createdCubes[i].CleanAllRunes();
                _dices[i].SetScriptableSettings(_createdCubes[i]);
            }
        }

        public void SetBalancedCubes()
        {
            _diceListSetupScriptable.SetListVariant(DiceListVariant.Balanced);
            GoToTeamCreationMenuAsync().Forget();
        }
        
        public void SetRandomCubes()
        {
            _diceListSetupScriptable.SetListVariant(DiceListVariant.FullRandom);
            GoToTeamCreationMenuAsync().Forget();
        }
        
        public void SetCreatedCubes()
        {
            _diceListSetupScriptable.SetListVariant(DiceListVariant.Created);
            GoToTeamCreationMenuAsync().Forget();
        }
        private async UniTask GoToTeamCreationMenuAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
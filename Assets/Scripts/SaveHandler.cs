using System;
using System.Collections.Generic;
using Enemies;
using PlayerGroup;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class SaveHandler : MonoBehaviour
    {
         private TestButtonUI.Factory _buttonFactory;
         private List<TestButtonUI> _buttons;
         private PlayerGroupModel _playerGroupModel;
         private EnemySpawner _enemySpawner;

         [Inject]
         private void Construct(TestButtonUI.Factory buttonFactory, PlayerGroupModel playerGroupModel, EnemySpawner enemySpawner)
         {
             _buttonFactory = buttonFactory;
             _playerGroupModel = playerGroupModel;
             _enemySpawner = enemySpawner;
         }

         private void Start()
         {
             Initialize();
         }
         public void Initialize()
         {
             _buttons = new List<TestButtonUI>();
             _buttons.Add(_buttonFactory.Create(Save, "Save"));
         }

         private void Save()
         {
             _playerGroupModel.SaveModelToJson();
             _enemySpawner.SaveEnemyModelsToJson();
         }
    }
}
using System.Collections.Generic;
using BuildingScripts;
using BuildingScripts.RecruitingBuildings;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Enemies;
using ScriptableScripts;
using UnityEngine;
using Zenject;

namespace GameEvents
{
    public class GameEventHandler : MonoBehaviour
    {
        private SaveHandler _saveHandler;
        [SerializeField]
        private SceneScriptableData _sceneScriptableData;

        [Inject]
        public void Construct(SaveHandler saveHandler)
        {
            _saveHandler = saveHandler;
        }
        
        
        private async UniTask SaveBeforeEventAsync()
        {
            SceneChanger.GetInstance().LoadScreenEnabled(true);
            await _saveHandler.SaveAsync();
        }
        public async UniTask FightAsync(EnemyModel enemyModel)
        {
            _sceneScriptableData.SetAttackedEnemyModel(enemyModel);
            await SaveBeforeEventAsync();
            SceneChanger.GetInstance().FightScene();
        }

        public async UniTask RecruitCenterVisitAsync(RecruitingCenterSetup recruitingCenterSetup)
        {
            _sceneScriptableData.SetRecruits(recruitingCenterSetup);
            _sceneScriptableData.LoadMap = true;//load
            await SaveBeforeEventAsync();
            SceneChanger.GetInstance().RecruitSceneBegin();
        }
        
    }
}

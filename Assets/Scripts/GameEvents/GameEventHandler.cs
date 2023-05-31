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

        public async UniTask RecruitCenterVisitAsync(RecruitingCenterProperty recruitingCenterProperty)
        {
            _sceneScriptableData.SetRecruits(recruitingCenterProperty);
            await SaveBeforeEventAsync();
            SceneChanger.GetInstance().RecruitSceneBegin();
        }
        
    }
}

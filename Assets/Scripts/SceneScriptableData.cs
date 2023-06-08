using System.Collections.Generic;
using BuildingScripts;
using BuildingScripts.RecruitingBuildings;
using Enemies;
using ScriptableScripts;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu (menuName = "SceneScriptableData")]
    public class SceneScriptableData : ScriptableObject
    {
        
        private EnemyModel _enemyModelAttacked;
        private RecruitingCenterSetup _recruitingCenterSetup;
        
        public bool LoadMap;



        public void SetAttackedEnemyModel(EnemyModel enemyModel)
        {
            _enemyModelAttacked = enemyModel;
        }
        public EnemyModel GetEnemyModelFromAttackEvent()
        {
            if (_enemyModelAttacked == null) return null;
            var enemy = _enemyModelAttacked;
            _enemyModelAttacked = null;
            return enemy;
        }

        public void SetRecruits(RecruitingCenterSetup recruits)
        {
            _recruitingCenterSetup = recruits;
        }
        public RecruitingCenterSetup GetRecruitingCenterProperty()
        {
            
            if (_recruitingCenterSetup == null)
            {
                return null;
            }
            var recruit = _recruitingCenterSetup;
            _recruitingCenterSetup = null;
            return recruit;
        }
    }
}
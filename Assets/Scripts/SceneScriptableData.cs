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
        private RecruitingCenterProperty _recruitingCenterProperty;
        
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

        public void SetRecruits(RecruitingCenterProperty recruits)
        {
            _recruitingCenterProperty = recruits;
        }
        public RecruitingCenterProperty GetRecruitingCenterProperty()
        {
            if (_recruitingCenterProperty == null) return null;
            var recruit = _recruitingCenterProperty;
            _recruitingCenterProperty = null;
            return recruit;
        }
    }
}
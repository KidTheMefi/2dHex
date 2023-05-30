using System;
using BuildingScripts;
using UI;
using UnityEngine;

[Serializable]
public class PrefabList
{
        [SerializeField] private HexView _hexViewPrefab;
        [SerializeField] private RiverView _riverPrefab;
        [SerializeField] private TestButtonUI _buttonUIPrefab;
        [SerializeField] private PlayerUIEnergy _playerUIEnergy;
        [SerializeField] private PlayerGroupView _playerGroupPrefab;
        [SerializeField] private PathPoint _pathPointPrefab;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private HexHighlight _hexHighlightPrefab;
        [SerializeField] private RecruitingCenter _recruitingCenterPrefab;

        public HexView HexViewPrefab => _hexViewPrefab;
        public TestButtonUI ButtonPrefab => _buttonUIPrefab;
        public RiverView RiverPrefab => _riverPrefab;
        public PlayerGroupView PlayerGroupPrefab => _playerGroupPrefab;
        public PathPoint PathPoint => _pathPointPrefab;
        public GameObject EnemyFacade => _enemyPrefab;
        public PlayerUIEnergy PlayerUIEnergy => _playerUIEnergy;
        public HexHighlight HexHighlight => _hexHighlightPrefab;
        public RecruitingCenter RecruitingCenterPrefab => _recruitingCenterPrefab;

}

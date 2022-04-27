using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabList
{
        [SerializeField] private HexView _hexViewPrefab;
        [SerializeField] private RiverView _riverPrefab;
        [SerializeField] private TestButtonUI _buttonUIPrefab;
        [SerializeField] private PlayerGroupView _playerGroupPrefab;
        [SerializeField] private PathPoint _pathPointPrefab;

        public HexView HexViewPrefab => _hexViewPrefab;
        public TestButtonUI ButtonPrefab => _buttonUIPrefab;
        public RiverView RiverPrefab => _riverPrefab;
        public PlayerGroupView PlayerGroupPrefab => _playerGroupPrefab;
        public PathPoint PathPoint => _pathPointPrefab;

}

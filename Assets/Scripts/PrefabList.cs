using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabList
{
        [SerializeField] private HexView _hexViewPrefab;
        [SerializeField] private RiverView _riverPrefab;
        [SerializeField] private GameObject _pathPointCircle;
        [SerializeField] private GameObject _startPathPointCircle;
        [SerializeField] private GameObject _endPathPointCircle;
        [SerializeField] private TestButtonUI _buttonUIPrefab;

        public HexView HexViewPrefab => _hexViewPrefab;
        public TestButtonUI ButtonPrefab => _buttonUIPrefab;
        public RiverView RiverPrefab => _riverPrefab;
        public GameObject PathPoint => _pathPointCircle;
        public GameObject StartPathPoint => _startPathPointCircle;
        public GameObject EndPathPoint => _endPathPointCircle;
    
}

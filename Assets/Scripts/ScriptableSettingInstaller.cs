using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "HexGame/BaseGameSettings")]
public class ScriptableSettingInstaller : ScriptableObjectInstaller<ScriptableSettingInstaller>
{
    [SerializeField]
    private HexMap.PrefabSettings HexMapPrefabs;
    [SerializeField]
    private MapSetting MapSetting;
    [SerializeField]
    private HexView HexViewPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(HexViewPrefab);
        Container.BindInstance(HexMapPrefabs);
        Container.BindInstance(MapSetting);
    }


}
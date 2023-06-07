using BuildingScripts;
using BuildingScripts.MapTargets;
using BuildingScripts.ObservationBuildings;
using BuildingScripts.RecruitingBuildings;
using BuildingScripts.RestPlaces;
using Enemies;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "HexGame/BaseGameSettings")]
public class ScriptableSettingInstaller : ScriptableObjectInstaller<ScriptableSettingInstaller>
{
    [SerializeField]
    private PrefabList Prefabs;
    [SerializeField]
    private MapSetting MapSetting;
    [SerializeField]
    private RecruitingCentersOnMap _recruitingCentersOnMap;
    [SerializeField]
    private TemplesOnMap _templesOnMap;
    [SerializeField]
    private RestingPlaceOnMap _restingPlaceOnMap;
    [SerializeField]
    private ObservationPlaceOnMap _observationPlaceOnMap;
    [SerializeField]
    private GameTime.InGameTime.Settings GameTimeSettings;
    [SerializeField]
    private EnemiesSettings _enemiesSettings;
    

    public override void InstallBindings()
    {
        Container.BindInstance(Prefabs);
        Container.BindInstance(MapSetting);
        Container.BindInstance(GameTimeSettings);
        Container.BindInstance(_enemiesSettings);
        Container.BindInstance(_recruitingCentersOnMap);
        Container.BindInstance(_templesOnMap);
        Container.BindInstance(_restingPlaceOnMap);
        Container.BindInstance(_observationPlaceOnMap);
    }
}
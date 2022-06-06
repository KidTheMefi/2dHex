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
    private GameTime.InGameTime.Settings GameTimeSettings;
    [SerializeField]
    private EnemiesSettings _enemiesSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(Prefabs);
        Container.BindInstance(MapSetting);
        Container.BindInstance(GameTimeSettings);
        Container.BindInstance(_enemiesSettings);
    }
}
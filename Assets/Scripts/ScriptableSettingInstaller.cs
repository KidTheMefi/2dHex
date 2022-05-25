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
    private GameTime.GameTime.Settings GameTimeSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(Prefabs);
        Container.BindInstance(MapSetting);
        Container.BindInstance(GameTimeSettings);
    }
}
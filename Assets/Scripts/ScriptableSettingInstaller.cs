using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "HexGame/BaseGameSettings")]
public class ScriptableSettingInstaller : ScriptableObjectInstaller<ScriptableSettingInstaller>
{
    [SerializeField]
    private PrefabList Prefabs;
    [SerializeField]
    private MapSetting MapSetting;
    

    public override void InstallBindings()
    {
        Container.BindInstance(Prefabs);
        Container.BindInstance(MapSetting);
    }
}
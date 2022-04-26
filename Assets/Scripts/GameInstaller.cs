using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform _hexHighlight;
    
    [Inject]
    private PrefabList _prefabList = null;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_camera).AsSingle();
        Container.BindInstance(_canvas).AsSingle();
        Container.BindInstance(_hexHighlight).WithId("hexHighlight").AsSingle();
        
        InstallInputSystem();
        InstallGameField();
        InstallPathFind();
        InstallMap();
        InstallPlayerGroup();
    }

    private void InstallInputSystem()
    {
        Container.BindInterfacesAndSelfTo<TestInputActions>().AsSingle();
        Container.BindInterfacesAndSelfTo<HexMouseController>().AsSingle();
    }

    private void InstallGameField()
    {
        Container.BindFactory<HexView, HexView.Factory>().FromComponentInNewPrefab(_prefabList.HexViewPrefab).WithGameObjectName("tile").UnderTransformGroup("HexGridMap");
        Container.BindFactory<Continent, Continent.Factory>();
        Container.BindFactory<TestButtonUI, TestButtonUI.Factory>().FromComponentInNewPrefab(_prefabList.ButtonPrefab).UnderTransform(_canvas.transform);
        Container.BindFactory<RiverView, RiverView.Factory>().FromMonoPoolableMemoryPool(
            x => x.FromComponentInNewPrefab(_prefabList.RiverPrefab).UnderTransformGroup("RiverPool"));

        Container.BindInterfacesAndSelfTo<HexMapGrid>().AsSingle();

    }

    private void InstallPlayerGroup()
    {
        Container.Bind<PlayerGroupView>().FromComponentInNewPrefab(_prefabList.PlayerGroupPrefab).WithGameObjectName("Player").AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerGroupModel>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerSelectControl>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerGroupMovement>().AsSingle();
    }
    
    private void InstallPathFind()
    {
        Container.BindInterfacesAndSelfTo<AStarSearch>().AsSingle();
    }
    
    private void InstallMap()
    {
        Container.BindInterfacesAndSelfTo<LandGeneration>().AsSingle();
        Container.BindInterfacesAndSelfTo<MapGeneration>().AsSingle();
        Container.BindInterfacesAndSelfTo<HexMapContinents>().AsSingle();
        Container.BindInterfacesAndSelfTo<RiverGenerator>().AsSingle();
    }
}

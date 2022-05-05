using Enemies;
using PlayerGroup;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _buttonsParent;
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
        InstallEnemies();
    }

    private void InstallInputSystem()
    {
        Container.BindInterfacesAndSelfTo<TestInputActions>().AsSingle();
        Container.BindInterfacesAndSelfTo<CameraMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<HexMouse>().AsSingle();
    }

    private void InstallGameField()
    {
        Container.BindFactory<HexView, HexView.Factory>().FromComponentInNewPrefab(_prefabList.HexViewPrefab).WithGameObjectName("tile").UnderTransformGroup("HexGridMap");
        Container.BindFactory<Continent, Continent.Factory>();
        Container.BindFactory<TestButtonUI, TestButtonUI.Factory>().FromComponentInNewPrefab(_prefabList.ButtonPrefab).UnderTransform(_buttonsParent.transform);
        
        
        Container.BindFactory<RiverView, RiverView.Factory>().FromMonoPoolableMemoryPool(
            x => x.WithInitialSize(15).FromComponentInNewPrefab(_prefabList.RiverPrefab).UnderTransformGroup("RiverPool"));
        
        Container.BindFactory<PathPoint, PathPoint.Factory>().FromMonoPoolableMemoryPool(
            x => x.WithInitialSize(50).FromComponentInNewPrefab(_prefabList.PathPoint).UnderTransformGroup("PathFindPool"));

        Container.BindInterfacesAndSelfTo<HexMapGrid>().AsSingle();

    }

    private void InstallPlayerGroup()
    {
        Container.Bind<PlayerGroupView>().FromComponentInNewPrefab(_prefabList.PlayerGroupPrefab).WithGameObjectName("Player").AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerGroupModel>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerGroupMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerPathFind>().AsSingle();
        Container.BindInterfacesAndSelfTo<FieldOfView>().AsSingle();
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
    private void InstallEnemies()
    {
        Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
        Container.BindFactory<Vector2Int, EnemyFacade, EnemyFacade.Factory>()
            .FromPoolableMemoryPool<Vector2Int, EnemyFacade, EnemyFacadePool>(poolBinder => poolBinder
                .WithInitialSize(3)
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<EnemyInstaller>(_prefabList.EnemyFacade)
                .UnderTransformGroup("Enemies"));
    }
}
 class EnemyFacadePool : MonoPoolableMemoryPool<Vector2Int, IMemoryPool, EnemyFacade>
{
}

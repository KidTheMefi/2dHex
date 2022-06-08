using System;
using Enemies;
using GameTime;
using PlayerGroup;
using UI;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _buttonsParent;
    [SerializeField] private Transform _hexHighlight;
    [SerializeField] private TimeClock _clock;
    [SerializeField] private NightMask _nightMask;
    [SerializeField] private PanelScript _panel;
    
    [Inject]
    private PrefabList _prefabList = null;
    
    public override void InstallBindings()
    {
        
        Container.BindInstance(_camera).AsSingle();
        Container.BindInstance(_canvas).AsSingle();
        Container.BindInstance(_hexHighlight).WithId("hexHighlight").AsTransient();
        Container.BindInstance(_clock).AsSingle();
        Container.BindInstance(_nightMask).AsSingle();
        Container.BindInstance(_panel).AsSingle();
        Container.Bind<HexHighlight>().FromComponentInNewPrefab(_prefabList.HexHighlight).WithGameObjectName("hexHighlight").AsSingle();

        Container.BindInterfacesAndSelfTo<GameTime.InGameTime>().AsSingle();
        InstallInputSystem();
        InstallGameField();
        InstallPathFind();
        InstallMap();
        InstallPlayerGroup();
        InstallEnemies();
        InstallUI();
        
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
        
        Container.Bind<PlayerGroupMovement>().AsSingle();
        Container.Bind<PlayerGroupIdle>().AsSingle();
        //Container.Bind(typeof(IInitializable),typeof(PlayerGroupIdle)).To<PlayerGroupIdle>().AsSingle();
        //Container.Bind(typeof(PlayerGroupIdle), typeof(IInitializable)).To<PlayerGroupIdle>().AsSingle();
        Container.Bind<PlayerGroupEvent>().AsSingle();
        Container.Bind<PlayerGroupRest>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<PlayerGroupSpawn>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerPathFind>().AsSingle();
        Container.BindInterfacesAndSelfTo<FieldOfView>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerGroupFacade>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerGroupStateManager>().AsSingle();
        
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
        Container.BindFactory<Vector2Int, EnemySettings, EnemyFacade, EnemyFacade.Factory>()
            .FromPoolableMemoryPool<Vector2Int,EnemySettings, EnemyFacade, EnemyFacadePool>(poolBinder => poolBinder
                .WithInitialSize(10)
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<EnemyInstaller>(_prefabList.EnemyFacade)
                .UnderTransformGroup("Enemies"));
    }

    private void InstallUI()
    {
        Container.BindInterfacesAndSelfTo<PauseMenu>().AsSingle();
        Container.BindFactory<Action, string, TestButtonUI, TestButtonUI.Factory>().FromComponentInNewPrefab(_prefabList.ButtonPrefab).UnderTransform(_buttonsParent.transform);
        Container.Bind<PlayerUIEnergy>().FromComponentInNewPrefab(_prefabList.PlayerUIEnergy).UnderTransform(_canvas.transform).AsSingle().NonLazy();
    }
}
 class EnemyFacadePool : MonoPoolableMemoryPool<Vector2Int, EnemySettings, IMemoryPool, EnemyFacade>
{
}

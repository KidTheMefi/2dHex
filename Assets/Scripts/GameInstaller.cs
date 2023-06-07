using System;
using BuildingScripts;
using BuildingScripts.ObservationBuildings;
using BuildingScripts.RecruitingBuildings;
using BuildingScripts.RestPlaces;
using DefaultNamespace;
using Enemies;
using GameEvents;
using GameEvents.MapObjectDescriptionSignal;
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
    [SerializeField] private SaveHandler _saveHandler;
    [SerializeField] private GameEventHandler _gameEventHandler;
    [SerializeField] private MapObjectDescriptionPlate _mapObjectDescriptionPlate;
    


    [SerializeField] private PlayerGroupModel.PlayerSettings _playerSettings;

    [Inject]
    private PrefabList _prefabList = null;

    public override void InstallBindings()
    {

        BindInstance();
        Container.BindInterfacesAndSelfTo<InGameTime>().AsSingle();
        InstallInputSystem();
        InstallGameField();
        InstallPathFind();
        InstallMap();
        InstallPlayerGroup();
        InstallEnemies();
        InstallUI();
        InstallSignals();
        InstallBuildings();
        InstallMapDescription();
    }

    private void BindInstance()
    {
        Container.BindInstance(_saveHandler).AsSingle();
        Container.BindInstance(_camera).AsSingle();
        Container.BindInstance(_canvas).AsSingle();
        Container.BindInstance(_hexHighlight).WithId("hexHighlight").AsTransient();
        Container.BindInstance(_clock).AsSingle();
        Container.BindInstance(_nightMask).AsSingle();
        Container.BindInstance(_panel).AsSingle();
        Container.BindInstance(_gameEventHandler).AsSingle();
        Container.Bind<HexHighlight>().FromComponentInNewPrefab(_prefabList.HexHighlight).WithGameObjectName("hexHighlight").AsSingle();
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
        Container.BindInstance(_playerSettings).AsSingle();
        Container.Bind<PlayerGroupView>().FromComponentInNewPrefab(_prefabList.PlayerGroupPrefab).WithGameObjectName("Player").AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerGroupModel>().AsSingle();

        Container.Bind<PlayerGroupMovement>().AsSingle();
        Container.Bind<PlayerGroupIdle>().AsSingle();

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


    private void InstallBuildings()
    {
        Container.BindFactory<RestingPlaceSetup, BaseBuilding, RestingPlace, RestingPlace.Factory>().AsSingle();
        Container.BindFactory<ObservationPlaceSetup, BaseBuilding, ObservationPlace, ObservationPlace.Factory>().AsSingle();
        
        Container.BindFactory<BaseBuilding.BaseBuildingSavedData, BaseBuilding, BaseBuilding.Factory>()
            .FromPoolableMemoryPool<BaseBuilding.BaseBuildingSavedData, BaseBuilding, BaseBuildingPool>(poolBinder => poolBinder
                .WithInitialSize(30).FromComponentInNewPrefab(_prefabList.BaseBuilding)
                .UnderTransformGroup("Buildings"));
        Container.BindInterfacesAndSelfTo<BuildingsHandler>().AsSingle();
        
        InstallRecruitingCenter();
        Container.BindInterfacesAndSelfTo<ObservationPlaceHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<TempleBuildingHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<RestingPlaceHandler>().AsSingle();
        
    }
    private void InstallRecruitingCenter()
    {
        Container.BindInterfacesAndSelfTo<RecruitingCentersHandler>().AsSingle();
        Container.BindFactory<RecruitingCenter.RecruitingCenterSavedData, RecruitingCenter, RecruitingCenter.Factory>()
            .FromPoolableMemoryPool<RecruitingCenter.RecruitingCenterSavedData, RecruitingCenter, RecruitingCenterPool>(poolBinder => poolBinder
                .WithInitialSize(20).FromComponentInNewPrefab(_prefabList.RecruitingCenterPrefab)
                .UnderTransformGroup("RecruitingCenters"));
    }
    
    private void InstallEnemies()
    {
        Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();
        Container.BindFactory<Vector2Int, EnemyModel.Properties, EnemyFacade, EnemyFacade.Factory>()
            .FromPoolableMemoryPool<Vector2Int, EnemyModel.Properties, EnemyFacade, EnemyFacadePool>(poolBinder => poolBinder
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

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<GameSignals.EnemyAttackSignal>();
        Container.DeclareSignal<GameSignals.RecruitingCenterVisitSignal>();
        Container.BindInterfacesTo<EnemyAttackEvent>().AsSingle();
        Container.BindInterfacesAndSelfTo<RecruitingCenterVisitEvent>().AsSingle();
    }

    private void InstallMapDescription()
    {
        Container.BindInterfacesAndSelfTo<MapObjectDescriptionSignal>().AsSingle();
        Container.BindInstance(_mapObjectDescriptionPlate);

        //Container.BindInterfacesAndSelfTo<MapObjectDescriptionPlate>().FromInstance(_mapObjectDescriptionPlate).AsSingle();
    }
}

class EnemyFacadePool : MonoPoolableMemoryPool<Vector2Int, EnemyModel.Properties, IMemoryPool, EnemyFacade>
{
}

class RecruitingCenterPool : MonoPoolableMemoryPool<RecruitingCenter.RecruitingCenterSavedData, IMemoryPool, RecruitingCenter>
{

}

class BaseBuildingPool : MonoPoolableMemoryPool<BaseBuilding.BaseBuildingSavedData, IMemoryPool, BaseBuilding>
{

}
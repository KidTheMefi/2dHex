using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    
    [Inject]
    private HexView _hexViewPrefab = null;
    public override void InstallBindings()
    {
        Container.BindInstance(_camera).AsSingle();
        InstallGameField();
        InstallPathFind();
        InstallMapGenerator();
        InstallMap();
    }

    private void InstallGameField()
    {
        Container.BindFactory<HexView, HexView.Factory>().FromComponentInNewPrefab(_hexViewPrefab).WithGameObjectName("tile").UnderTransformGroup("HexGridMap");
        Container.BindFactory<Continent, Continent.Factory>();

        
        Container.BindInterfacesAndSelfTo<HexMapGrid>().AsSingle();

        //IHexStorage mapGrid = Instantiate(_settings.HexMapGridPrefab);
        //Container.BindInstance(mapGrid).AsSingle();
    }

    private void InstallPathFind()
    {
        Container.BindInterfacesAndSelfTo<AStarSearch>().AsSingle();
    }
    
    private void InstallMapGenerator()
    {
        //Container.BindInterfacesAndSelfTo<LandGeneration>().AsSingle();
        Container.Bind<ILandGeneration>().To<LandGeneration>().AsSingle();
    }

    private void InstallMap()
    {
        Container.BindInterfacesAndSelfTo<HexMap>().AsSingle();
    }
}

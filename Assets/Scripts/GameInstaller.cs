using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;

    [Inject]
    private PrefabList _prefabList = null;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_camera).AsSingle();
        Container.BindInstance(_canvas).AsSingle();
        InstallGameField();
        InstallPathFind();
        InstallMapGenerator();
        InstallMap();
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

    private void InstallPathFind()
    {
        Container.BindInterfacesAndSelfTo<AStarSearch>().AsSingle();
    }
    
    private void InstallMapGenerator()
    {
        Container.BindInterfacesAndSelfTo<LandGeneration>().AsSingle();
        //Container.Bind<ILandGeneration>().To<LandGeneration>().AsSingle();
    }

    private void InstallMap()
    {
        Container.BindInterfacesAndSelfTo<HexMapContinents>().AsSingle();
        Container.BindInterfacesAndSelfTo<RiverGenerator>().AsSingle();
    }
}

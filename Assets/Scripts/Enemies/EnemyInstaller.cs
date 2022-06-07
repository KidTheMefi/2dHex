using Enemies.EnemyStates;
using Zenject;

namespace Enemies
{
    public class EnemyInstaller : Installer<EnemyInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemyModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyStateManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyRest>().AsSingle();
        }
    }
}

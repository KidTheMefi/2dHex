using Zenject;

namespace Enemies
{
    public class EnemyInstaller : Installer<EnemyInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemyModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyMovement>().AsSingle();
        }
    }
}

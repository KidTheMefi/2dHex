using Cysharp.Threading.Tasks;

namespace Enemies.EnemyStates
{
    public class EnemyRest : IEnemyState
    {

        private EnemyModel _enemyModel;
        private EnemyView _enemyView;

        public EnemyRest(EnemyView enemyView, EnemyModel enemyModel)
        {
            _enemyView = enemyView;
            _enemyModel = enemyModel;
        }

        public void EnterState()
        {
            throw new System.NotImplementedException();
        }
        public void ExitState()
        {
            throw new System.NotImplementedException();
        }
        public UniTask OnGameTick()
        {
            throw new System.NotImplementedException();
        }
    }
}

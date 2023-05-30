using BuildingScripts;
using Enemies;

namespace GameEvents
{
    public class GameSignals 
    {
        public class EnemyAttackSignal
        {
            public EnemyModel EnemyModel;
        }
        
        public class RecruitingCenterVisitSignal
        {
            public RecruitingCenter RecruitingCenter;
        }
    }
}

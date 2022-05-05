using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyModel 
    {
        public EnemyModel()
        {
            
        }
        private Vector2Int _axialPosition;
        private int _viewRadius = 6;

        public int ViewRadius => _viewRadius;


        public Vector2Int AxialPosition
        {
            get { return _axialPosition; }
            set { _axialPosition = value; }
        }
    }
}

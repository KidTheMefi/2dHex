using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SceneChanger : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void AddFightScene()
        {
            
        }
    }
}
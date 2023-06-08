using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class SceneChanger : MonoBehaviour
    {
        private static SceneChanger _instance;
        
        [SerializeField]
        private SceneChanger _sceneChangerPrefab;
        [SerializeField]
        private GameObject _loadImage;
        [SerializeField]
        private SceneScriptableData _sceneScriptableData;
        
        public static SceneChanger GetInstance()
        {
            if (_instance == null)
            {
                var instance = Instantiate(Resources.Load<SceneChanger>("SceneChanger"));
                if (instance == null)
                {
                    throw new NullReferenceException();
                }
                _instance = instance;
            }
            return _instance;
           
        }
        
        private void Awake()
        {
            //SAVE FOLDER IN BUILD CREATE!!!
            if (!Directory.Exists(Application.dataPath + "/Save"))
            {
                Directory.CreateDirectory(Application.dataPath + "/Save");   
            }

            DontDestroyOnLoad(this);
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void NewGameBegin()
        {
            _loadImage.SetActive(true);
            SceneManager.LoadSceneAsync("TeamCreationScene", LoadSceneMode.Single);
        }

        public void RecruitSceneBegin()
        {
            Debug.Log("RecruitSceneBegin");
            SceneManager.LoadSceneAsync("TeamCreationScene", LoadSceneMode.Single);
        }
        
        public void FightScene()
        {
            _loadImage.SetActive(true);
            SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
        }

        public void CreateNewMap()
        {
            _loadImage.SetActive(true);
            _sceneScriptableData.LoadMap = false;
            SceneManager.LoadSceneAsync("SpriteHex", LoadSceneMode.Single);
        }
        
        public void BackToStartMenu()
        {
            SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
        }
        
        public void LoadGame()
        {
            _loadImage.SetActive(true);
            _sceneScriptableData.LoadMap = true;
            SceneManager.LoadSceneAsync("SpriteHex", LoadSceneMode.Single);
        }

        public void LoadScreenEnabled(bool value)
        {
            _loadImage.SetActive(value);
        }
    }
}
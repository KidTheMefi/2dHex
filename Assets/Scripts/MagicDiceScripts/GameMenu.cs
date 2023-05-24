using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    
    public void GoToTeamCreationScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    
    public void GoToFightScene()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("Quit");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
                   #endif
        Application.Quit();
    }
}

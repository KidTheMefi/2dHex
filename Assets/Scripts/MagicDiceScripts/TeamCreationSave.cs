using UnityEngine;

public class TeamCreationSave : MonoBehaviour
{
  
    public void ExitGame()
    {
        Debug.Log("Quit");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
                   #endif
        Application.Quit();
    }
}
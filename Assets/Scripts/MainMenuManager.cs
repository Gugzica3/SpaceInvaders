using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "SpaceInvadersScene";

    public void JogarGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void SairGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
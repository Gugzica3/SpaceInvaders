using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach to any UI Button that should restart the main game scene.
/// Works both from the main scene and from Victory/Defeat scenes.
/// </summary>
public class RestartLevel : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
            btn.onClick.AddListener(RestartGame);
    }

    void RestartGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
        else
        {
            // Fallback when called from Victory/Defeat scenes (no GameManager present)
            UnityEngine.SceneManagement.SceneManager.LoadScene("SpaceInvadersScene");
        }
    }
}

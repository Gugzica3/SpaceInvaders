using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Attach to the root object of the Defeat scene.
/// Reads the final score from PlayerPrefs and shows it.
/// </summary>
public class DefeatUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI titleText;

    void Start()
    {
        int score = PlayerPrefs.GetInt("FinalScore", 0);

        if (titleText != null)
            titleText.text = "GAME OVER";

        if (finalScoreText != null)
            finalScoreText.text = "Pontuacao: " + score;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}

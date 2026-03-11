using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Comportamento em runtime da MainMenuScene.
/// Anexado automaticamente ao UIRoot pelo MainMenuGenerator.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("Referências")]
    public TextMeshProUGUI insertCoinText;
    public GameObject      creditsPanel;

    [Header("Configuração")]
    public string gameSceneName = "SpaceInvadersScene";

    private float _blinkTimer;
    private const float BlinkInterval = 0.6f;

    void Update()
    {
        // Pisca o texto
        if (insertCoinText != null)
        {
            _blinkTimer += Time.deltaTime;
            if (_blinkTimer >= BlinkInterval)
            {
                _blinkTimer = 0f;
                insertCoinText.enabled = !insertCoinText.enabled;
            }
        }

        // Atalhos de teclado
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            if (creditsPanel == null || !creditsPanel.activeSelf)
                StartGame();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (creditsPanel != null && creditsPanel.activeSelf)
                ToggleCredits();
            else
                QuitGame();
        }
    }

    public void StartGame()     => SceneManager.LoadScene(gameSceneName);
    public void ToggleCredits() => creditsPanel?.SetActive(!creditsPanel.activeSelf);
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
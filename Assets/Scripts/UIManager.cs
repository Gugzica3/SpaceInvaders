using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD References (deixe vazio para criar automaticamente)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        EnsureHUD();
        UpdateScoreUI(0);
    }

    // Cria os textos automaticamente se não estiverem atribuídos
    void EnsureHUD()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        // Força o canvas para ScreenSpaceOverlay para não depender da câmera
        canvas.renderMode  = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        if (scoreText == null)
        {
            scoreText = FindOrCreateTMP(canvas, "ScoreTextTMP",
                new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1),
                new Vector2(20, -20), new Vector2(350, 80),
                TextAlignmentOptions.TopLeft);
        }

        if (livesText == null)
        {
            livesText = FindOrCreateTMP(canvas, "LivesTextTMP",
                new Vector2(1, 1), new Vector2(1, 1), new Vector2(1, 1),
                new Vector2(-20, -20), new Vector2(350, 80),
                TextAlignmentOptions.TopRight);
        }

        // Garante que os textos ficam na frente da borda de arcade
        if (scoreText != null) scoreText.transform.SetAsLastSibling();
        if (livesText != null) livesText.transform.SetAsLastSibling();
    }

    TextMeshProUGUI FindOrCreateTMP(Canvas canvas, string objName,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
        Vector2 anchoredPos, Vector2 size, TextAlignmentOptions align)
    {
        // Tenta encontrar um já existente pelo nome
        Transform existing = canvas.transform.Find(objName);
        if (existing != null)
        {
            var found = existing.GetComponent<TextMeshProUGUI>();
            if (found != null) return found;
        }

        // Cria novo
        GameObject obj = new GameObject(objName);
        obj.transform.SetParent(canvas.transform, false);
        obj.transform.SetAsLastSibling();

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.alignment          = align;
        tmp.color              = Color.yellow;
        tmp.fontSize           = 48;
        tmp.enableWordWrapping = false;
        tmp.fontStyle          = FontStyles.Bold;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin        = anchorMin;
        rt.anchorMax        = anchorMax;
        rt.pivot            = pivot;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta        = size;

        return tmp;
    }

    public void UpdateScoreUI(int score)
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void UpdateLivesUI(int lives)
    {
        if (livesText != null) livesText.text = "Vidas: " + lives;
    }
}

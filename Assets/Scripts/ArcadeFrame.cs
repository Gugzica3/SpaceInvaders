using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Draws an arcade cabinet frame around the game viewport using UI Images.
/// Attach to the Canvas (or a child GameObject of the Canvas).
/// The "game area" is a centered black rectangle; the borders are styled panels.
/// </summary>
public class ArcadeFrame : MonoBehaviour
{
    [Header("Frame Colors")]
    public Color leftColor   = new Color(0.08f, 0.08f, 0.40f, 1f);   // dark blue cabinet sides
    public Color rightColor  = new Color(0.08f, 0.08f, 0.40f, 1f);
    public Color topColor    = new Color(0.05f, 0.05f, 0.05f, 1f);   // near-black top bezel
    public Color bottomColor = new Color(0.10f, 0.06f, 0.00f, 1f);   // dark wood-ish bottom

    [Header("Border Sizes (pixels at 1920x1080)")]
    public float sideWidth   = 130f;
    public float topHeight   =  80f;
    public float bottomHeight = 100f;

    [Header("Decorations")]
    public Color scanlineColor = new Color(0f, 0f, 0f, 0.06f);  // subtle scanline tint
    public Color titleColor    = new Color(1f, 0.85f, 0.05f, 1f);

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null) return;

        BuildFrame();
    }

    void BuildFrame()
    {
        // ── Borders ──────────────────────────────────────────────────────────
        CreateBorder("Frame_Left",
            new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(0, 0), new Vector2(sideWidth, 0),
            leftColor);

        CreateBorder("Frame_Right",
            new Vector2(1, 0), new Vector2(1, 1),
            new Vector2(1, 1), new Vector2(sideWidth, 0),
            rightColor);

        CreateBorder("Frame_Top",
            new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0.5f, 1f), new Vector2(0, topHeight),
            topColor);

        CreateBorder("Frame_Bottom",
            new Vector2(0, 0), new Vector2(1, 0),
            new Vector2(0.5f, 0f), new Vector2(0, bottomHeight),
            bottomColor);

        // ── Vertical accent lines (silver/grey trim) ─────────────────────────
        CreateAccentLine("Accent_Left",  sideWidth,       true);
        CreateAccentLine("Accent_Right", sideWidth,       false);

        // ── "SPACE INVADERS" title text in top bezel ─────────────────────────
        CreateTitleText();

        // ── Score label baked into bottom bezel ─────────────────────────────
        CreateBottomDecor();

        // ── Scanline overlay (very subtle) ───────────────────────────────────
        CreateScanlineOverlay();
    }

    // ─────────────────────────────────────────────────────────────────────────

    void CreateBorder(string name,
        Vector2 anchorMin, Vector2 anchorMax,
        Vector2 pivot, Vector2 sizeDelta,
        Color color)
    {
        GameObject obj = new GameObject(name, typeof(Image));
        obj.transform.SetParent(transform, false);
        obj.GetComponent<Image>().color = color;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin       = anchorMin;
        rt.anchorMax       = anchorMax;
        rt.pivot           = pivot;
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta       = sizeDelta;
    }

    void CreateAccentLine(string name, float fromEdge, bool isLeft)
    {
        GameObject obj = new GameObject(name, typeof(Image));
        obj.transform.SetParent(transform, false);
        obj.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.7f, 1f);  // silver trim

        RectTransform rt = obj.GetComponent<RectTransform>();
        if (isLeft)
        {
            rt.anchorMin       = new Vector2(0, 0);
            rt.anchorMax       = new Vector2(0, 1);
            rt.pivot           = new Vector2(0, 0.5f);
            rt.anchoredPosition = new Vector2(fromEdge, 0);
        }
        else
        {
            rt.anchorMin       = new Vector2(1, 0);
            rt.anchorMax       = new Vector2(1, 1);
            rt.pivot           = new Vector2(1, 0.5f);
            rt.anchoredPosition = new Vector2(-fromEdge, 0);
        }
        rt.sizeDelta = new Vector2(4f, 0);
    }

    void CreateTitleText()
    {
        // Use legacy Text (safe — no TMP dependency in ArcadeFrame)
        GameObject obj = new GameObject("ArcadeTitle", typeof(Text));
        obj.transform.SetParent(transform, false);

        Text t = obj.GetComponent<Text>();
        t.text      = "SPACE  INVADERS";
        t.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize  = 38;
        t.fontStyle = FontStyle.Bold;
        t.color     = titleColor;
        t.alignment = TextAnchor.MiddleCenter;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin       = new Vector2(0, 1);
        rt.anchorMax       = new Vector2(1, 1);
        rt.pivot           = new Vector2(0.5f, 1f);
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta       = new Vector2(0, topHeight);
    }

    void CreateBottomDecor()
    {
        // A subtle "INSERT COIN" or decorative text in the bottom panel
        GameObject obj = new GameObject("BottomDecor", typeof(Text));
        obj.transform.SetParent(transform, false);

        Text t = obj.GetComponent<Text>();
        t.text      = "< < <   SPACE INVADERS   > > >";
        t.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize  = 22;
        t.color     = new Color(1f, 0.6f, 0.05f, 0.8f);
        t.alignment = TextAnchor.MiddleCenter;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin       = new Vector2(0, 0);
        rt.anchorMax       = new Vector2(1, 0);
        rt.pivot           = new Vector2(0.5f, 0);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta       = new Vector2(0, bottomHeight);
    }

    void CreateScanlineOverlay()
    {
        // A dark semi-transparent overlay that sits above the game but below the frame
        GameObject obj = new GameObject("Scanlines", typeof(Image));
        obj.transform.SetParent(transform, false);
        obj.transform.SetSiblingIndex(0); // Put behind frame borders

        Image img = obj.GetComponent<Image>();
        img.color = scanlineColor;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin       = Vector2.zero;
        rt.anchorMax       = Vector2.one;
        rt.sizeDelta       = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
    }
}

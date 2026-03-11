#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuGenerator
{
    [MenuItem("Space Invaders/Gerar MainMenuScene")]
    public static void Generate()
    {
        // Garante pasta
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Camera
        GameObject camObj = new GameObject("Main Camera");
        camObj.tag = "MainCamera";
        camObj.AddComponent<AudioListener>();
        Camera cam = camObj.AddComponent<Camera>();
        cam.orthographic     = true;
        cam.orthographicSize = 6f;
        cam.clearFlags       = CameraClearFlags.SolidColor;
        cam.backgroundColor  = Color.black;
        camObj.transform.position = new Vector3(0, 0, -10f);

        // Canvas
        GameObject canvasObj = new GameObject("Canvas",
            typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode    = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera   = cam;
        canvas.planeDistance = 5f;
        CanvasScaler cs = canvasObj.GetComponent<CanvasScaler>();
        cs.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920, 1080);
        cs.matchWidthOrHeight  = 0.5f;

        // EventSystem
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        // Título
        GameObject titleObj = new GameObject("TitleText", typeof(TextMeshProUGUI));
        titleObj.transform.SetParent(canvasObj.transform, false);
        var title = titleObj.GetComponent<TextMeshProUGUI>();
        title.text      = "SPACE INVADERS";
        title.fontSize  = 90;
        title.color     = new Color(0.2f, 1f, 0.3f);
        title.alignment = TextAlignmentOptions.Center;
        title.fontStyle = FontStyles.Bold;
        var titleRt = titleObj.GetComponent<RectTransform>();
        titleRt.anchorMin = titleRt.anchorMax = titleRt.pivot = new Vector2(0.5f, 0.5f);
        titleRt.anchoredPosition = new Vector2(0, 280);
        titleRt.sizeDelta = new Vector2(1200, 150);

        // Botão JOGAR
        MakeButton(canvasObj, "BtnJogar", "JOGAR",
            new Vector2(0, 50), new Vector2(350, 80), new Color(0.05f, 0.4f, 0.05f));

        // Botão CREDITOS
        MakeButton(canvasObj, "BtnCreditos", "CREDITOS",
            new Vector2(0, -60), new Vector2(350, 80), new Color(0.05f, 0.1f, 0.4f));

        // Botão SAIR
        MakeButton(canvasObj, "BtnSair", "SAIR",
            new Vector2(0, -170), new Vector2(350, 80), new Color(0.4f, 0.05f, 0.05f));

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainMenuScene.unity");

        // Build Settings
        string path = "Assets/Scenes/MainMenuScene.unity";
        var builds = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        if (!builds.Exists(s => s.path == path))
        {
            builds.Insert(0, new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = builds.ToArray();
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene(path);
        Debug.Log("[SpaceInvaders] MainMenuScene gerada!");
    }

    static void MakeButton(GameObject canvas, string name, string label, Vector2 pos, Vector2 size, Color color)
    {
        GameObject btn = new GameObject(name, typeof(Image), typeof(Button));
        btn.transform.SetParent(canvas.transform, false);
        btn.GetComponent<Image>().color = color;
        var rt = btn.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;

        GameObject lbl = new GameObject("Label", typeof(TextMeshProUGUI));
        lbl.transform.SetParent(btn.transform, false);
        var tmp = lbl.GetComponent<TextMeshProUGUI>();
        tmp.text      = label;
        tmp.fontSize  = 42;
        tmp.color     = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        var lblRt = lbl.GetComponent<RectTransform>();
        lblRt.anchorMin = Vector2.zero;
        lblRt.anchorMax = Vector2.one;
        lblRt.sizeDelta = Vector2.zero;
    }
}
#endif
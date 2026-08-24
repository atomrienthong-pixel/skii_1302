using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Puts the main menu back into one tidy stack: the Settings button moves in
/// with Start and Exit instead of floating loose on the Canvas, and the panel
/// gets a plain centred rect instead of the dragged-out stretch values.
/// </summary>
static class MenuLayoutFix
{
    const string ScenePath = "Assets/Scenes/MainMenu.unity";

    static readonly Vector2 PanelSize = new Vector2(480f, 440f);
    static readonly Vector2 PanelPosition = new Vector2(0f, -60f);
    static readonly Vector2 ButtonSize = new Vector2(390f, 100f);

    // One column, tightly stacked, in the order they read on screen.
    const float StartY = 120f;
    const float SettingsY = 0f;
    const float ExitY = -120f;

    /// <summary>The title was overflowing off the top of the screen.</summary>
    const float TitleY = 370f;

    [MenuItem("Tools/Ski/Fix Main Menu Layout")]
    public static void Run()
    {
        // Opening a scene throws away whatever is unsaved in the current one, so
        // give the editor a chance to ask first. Batchmode has nothing to lose
        // and returns true straight away.
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            Debug.Log("[MenuLayoutFix] Cancelled; nothing changed.");
            return;
        }

        var scene = EditorSceneManager.OpenScene(ScenePath);

        var panel = Find("Panel");
        var start = Find("StartButton");
        var exit = Find("ExitButton");
        var settings = Find("SettingsButton");
        var settingsPanel = Find("SettingsPanel");

        if (panel == null || start == null || exit == null || settings == null)
        {
            Debug.LogError("[MenuLayoutFix] Could not find every menu object; nothing changed.");
            return;
        }

        var canvas = panel.transform.parent;

        var panelRect = (RectTransform)panel.transform;
        Centre(panelRect);
        panelRect.sizeDelta = PanelSize;
        panelRect.anchoredPosition = PanelPosition;

        // The Settings button lived on the Canvas, so it drifted away from the
        // panel it belongs to. worldPositionStays: false keeps the local rect.
        settings.transform.SetParent(panel.transform, false);

        Place(start, StartY);
        Place(settings, SettingsY);
        Place(exit, ExitY);

        // The title is the only other direct child of the Canvas, and it is
        // named "Text (TMP)" like several unrelated labels, so find it by where
        // it sits rather than by name.
        var title = OtherCanvasChild(canvas, panel, settings, settingsPanel);
        if (title != null)
            title.anchoredPosition = new Vector2(title.anchoredPosition.x, TitleY);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[MenuLayoutFix] Settings moved into the panel with Start and Exit; " +
                  $"panel recentred{(title != null ? " and title lowered" : string.Empty)}. Saved.");
    }

    static RectTransform OtherCanvasChild(Transform canvas, params GameObject[] known)
    {
        foreach (Transform child in canvas)
        {
            bool isKnown = false;
            foreach (var go in known)
                isKnown |= go != null && go.transform == child;

            if (!isKnown && child is RectTransform rect)
                return rect;
        }

        return null;
    }

    static void Place(GameObject go, float y)
    {
        var rect = (RectTransform)go.transform;
        Centre(rect);
        rect.sizeDelta = ButtonSize;
        rect.anchoredPosition = new Vector2(0f, y);
    }

    static void Centre(RectTransform rect)
    {
        rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
        rect.localScale = Vector3.one;
    }

    /// <summary>Names in this scene carry stray spaces, so match on the trimmed name.</summary>
    static GameObject Find(string name)
    {
        foreach (var rect in Object.FindObjectsByType<RectTransform>(
                     FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (rect.name.Trim() == name)
                return rect.gameObject;
        }

        return null;
    }
}

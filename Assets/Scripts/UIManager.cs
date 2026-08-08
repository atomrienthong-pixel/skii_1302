using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    [SerializeField]
    private TMP_Text hpText;

    [SerializeField]
    private TMP_Text damageText;

    [SerializeField]
    private TMP_Text notiText;

    [SerializeField]
    private float damageShowDuration = 1f;

    [SerializeField]
    private float notiShowDuration = 1.5f;

    [Header("Game Over")]
    // ปล่อยว่างไว้ได้ ถ้าว่างจะสร้างจอ Game Over ให้เองตอนรัน
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TMP_Text gameOverText;

    [SerializeField]
    private Button restartButton;

    private Coroutine damageRoutine;
    private Coroutine notiRoutine;

    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        // กันกรณีออกจาก Play Mode ตอนเกมหยุดอยู่ แล้วค่า 0 ค้างมารอบถัดไป
        Time.timeScale = 1f;

        if (damageText != null)
            damageText.gameObject.SetActive(false);

        if (notiText != null)
            notiText.gameObject.SetActive(false);

        if (gameOverPanel == null)
            BuildFallbackGameOverUI();

        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(Restart);
            restartButton.onClick.AddListener(Restart);
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // อัปเดตเลือดที่แสดงบนจอ
    public void ShowHP(int hp)
    {
        if (hpText != null)
            hpText.text = "HP: " + hp;
    }

    // โชว์ดาเมจที่เพิ่งโดน แล้วซ่อนเองหลังครบเวลา
    public void ShowDamage(int damage)
    {
        if (damageText == null)
            return;

        damageText.text = "-" + damage;
        damageText.gameObject.SetActive(true);

        if (damageRoutine != null)
            StopCoroutine(damageRoutine);

        damageRoutine = StartCoroutine(HideAfterDelay(damageText, damageShowDuration, () => damageRoutine = null));
    }

    // ข้อความแจ้งเตือนกลางจอ ซ่อนเองหลังครบเวลา
    public void ShowNotiText(string s)
    {
        if (notiText == null)
            return;

        notiText.text = s;
        notiText.gameObject.SetActive(true);

        if (notiRoutine != null)
            StopCoroutine(notiRoutine);

        notiRoutine = StartCoroutine(HideAfterDelay(notiText, notiShowDuration, () => notiRoutine = null));
    }

    public void ClearNotiText()
    {
        if (notiRoutine != null)
        {
            StopCoroutine(notiRoutine);
            notiRoutine = null;
        }

        if (notiText != null)
            notiText.gameObject.SetActive(false);
    }

    private IEnumerator HideAfterDelay(TMP_Text target, float duration, System.Action onDone)
    {
        // ใช้ Realtime เพราะตอน Game Over เราหยุดเวลาเกมไว้ (timeScale = 0)
        yield return new WaitForSecondsRealtime(duration);

        if (target != null)
            target.gameObject.SetActive(false);

        onDone?.Invoke();
    }

    // หยุดเกมแล้วเปิดจอสรุปพร้อมปุ่ม Restart
    public void ShowGameOver(string message)
    {
        if (IsGameOver)
            return;

        IsGameOver = true;
        Time.timeScale = 0f;

        ClearNotiText();

        if (gameOverText != null)
            gameOverText.text = message;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ---- จอ Game Over สำรอง สร้างตอนรันเมื่อไม่ได้ลาก panel ใส่ Inspector ----

    private void BuildFallbackGameOverUI()
    {
        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            GameObject canvasGo = new GameObject(
                "GameOverCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

            canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }

        gameOverPanel = new GameObject(
            "GameOverPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

        RectTransform panelRect = gameOverPanel.GetComponent<RectTransform>();
        panelRect.SetParent(canvas.transform, false);
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        gameOverPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.75f);

        if (gameOverText == null)
            gameOverText = CreateLabel("GameOverText", panelRect, new Vector2(0f, 90f), new Vector2(800f, 160f), 72f);

        if (restartButton == null)
            restartButton = CreateButton("RestartButton", panelRect, new Vector2(0f, -60f), new Vector2(280f, 90f), "Restart");
    }

    private TMP_Text CreateLabel(string name, RectTransform parent, Vector2 position, Vector2 size, float fontSize)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer));

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        TextMeshProUGUI label = go.AddComponent<TextMeshProUGUI>();
        label.fontSize = fontSize;
        label.alignment = TextAlignmentOptions.Center;
        label.color = Color.white;
        label.raycastTarget = false;

        return label;
    }

    private Button CreateButton(string name, RectTransform parent, Vector2 position, Vector2 size, string label)
    {
        GameObject go = new GameObject(
            name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        go.GetComponent<Image>().color = new Color(0.9f, 0.9f, 0.9f, 1f);

        TMP_Text buttonLabel = CreateLabel(name + "Label", rect, Vector2.zero, size, 36f);
        buttonLabel.color = Color.black;

        return go.GetComponent<Button>();
    }
}

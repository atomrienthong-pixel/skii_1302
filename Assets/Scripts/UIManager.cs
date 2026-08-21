using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private TMP_Text hpText;

    [SerializeField]
    private TMP_Text pointText;

    [SerializeField]
    private TMP_Text notiText;

    [SerializeField]
    private TMP_Text damageText;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TMP_Text gameOverText;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private float notiShowDuration = 1.5f;

    [SerializeField]
    private float damageShowDuration = 1f;

    private Coroutine notiRoutine;
    private Coroutine damageRoutine;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (notiText != null)
            notiText.gameObject.SetActive(false);

        if (damageText != null)
            damageText.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(Restart);
    }

    public void ShowHP(int hp)
    {
        if (hpText != null)
            hpText.text = "HP: " + hp;
    }

    public void ShowPoint(int point)
    {
        if (pointText != null)
            pointText.text = "Coin: " + point;
    }

    public void ShowNotiText(string message)
    {
        if (notiText == null)
            return;

        notiText.text = message;
        notiText.gameObject.SetActive(true);

        if (notiRoutine != null)
            StopCoroutine(notiRoutine);

        notiRoutine = StartCoroutine(HideAfter(notiText.gameObject, notiShowDuration));
    }

    public void ShowDamage(int damage)
    {
        if (damageText == null)
            return;

        damageText.text = "-" + damage;
        damageText.gameObject.SetActive(true);

        if (damageRoutine != null)
            StopCoroutine(damageRoutine);

        damageRoutine = StartCoroutine(HideAfter(damageText.gameObject, damageShowDuration));
    }

    private IEnumerator HideAfter(GameObject target, float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        target.SetActive(false);
    }

    public void ShowGameOver(string message)
    {
        if (gameOverText != null)
            gameOverText.text = message;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

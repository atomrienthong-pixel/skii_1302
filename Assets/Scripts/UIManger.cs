using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManger : MonoBehaviour
{
    [SerializeField]
    private TMP_Text notiText;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TMP_Text gameOverText;

    [SerializeField]
    private Button restartButton;


    public static UIManger instance;

    private void Awake()
    {
        instance = this;

        // กันค่า 0 ค้างจากรอบก่อน ถ้าเผลอหยุด Play ตอนเกม pause อยู่
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(Restart);
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNotiText(string s)
    {
        notiText.text = s;
    }

    // เลือดหมด: หยุดเกมแล้วเปิดจอ Game Over พร้อมปุ่ม Restart
    public void ShowGameOver(string message)
    {
        if (gameOverText != null)
            gameOverText.text = message;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // ปุ่ม Restart เรียกอันนี้
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }










}

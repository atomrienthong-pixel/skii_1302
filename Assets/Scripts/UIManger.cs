using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManger : MonoBehaviour
{
    [SerializeField]
    private TMP_Text notiText;

    [SerializeField]
    private TMP_Text pointText;

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

        if (gameOverPanel == null && gameOverText == null && restartButton == null)
            Debug.LogWarning("UIManger: ยังไม่ได้ลากอะไรใส่ช่อง Game Over เลย จอ Restart จะซ่อนไม่ได้", this);

        SetGameOverVisible(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(Restart);
    }

    // ซ่อน/โชว์ทีละชิ้น เผื่อปุ่มกับข้อความไม่ได้เป็นลูกของ panel
    private void SetGameOverVisible(bool visible)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(visible);

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(visible);

        if (restartButton != null)
            restartButton.gameObject.SetActive(visible);
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

    // อัปเดตจำนวนเหรียญที่เก็บได้
    public void ShowPoint(int point)
    {
        if (pointText != null)
            pointText.text = "Coin: " + point;
    }

    // เลือดหมด: หยุดเกมแล้วเปิดจอ Game Over พร้อมปุ่ม Restart
    public void ShowGameOver(string message)
    {
        SetGameOverVisible(true);

        if (gameOverText != null)
            gameOverText.text = message;

        Time.timeScale = 0f;
    }

    // ปุ่ม Restart เรียกอันนี้
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }










}

using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private TMP_Text hpText;

    [SerializeField]
    private TMP_Text damageText;

    [SerializeField]
    private float damageShowDuration = 1f;

    private Coroutine damageRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (damageText != null)
            damageText.gameObject.SetActive(false);
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

        damageRoutine = StartCoroutine(HideDamageAfterDelay());
    }

    private IEnumerator HideDamageAfterDelay()
    {
        yield return new WaitForSeconds(damageShowDuration);
        damageText.gameObject.SetActive(false);
        damageRoutine = null;
    }
}

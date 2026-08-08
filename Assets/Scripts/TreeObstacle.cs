using UnityEngine;

public class TreeObstacle : MonoBehaviour
{
    [SerializeField]
    private Color hitColor = Color.red;

    [SerializeField]
    private int damage = 1;

    // กันชนซ้ำรัวๆ ตอนผู้เล่นถูกับต้นไม้
    [SerializeField]
    private float hitCooldown = 0.5f;

    [SerializeField]
    private Renderer treeRenderer;

    private Color originalColor;
    private float lastHitTime = float.NegativeInfinity;

    void Start()
    {
        if (treeRenderer == null)
            treeRenderer = GetComponentInChildren<Renderer>();

        if (treeRenderer == null)
        {
            Debug.LogWarning("TreeObstacle: ไม่เจอ Renderer ต้นไม้จะไม่เปลี่ยนสีตอนชน", this);
            return;
        }

        originalColor = treeRenderer.material.color;
    }

    // ชนแบบ collider ปกติ (Is Trigger ปิด)
    private void OnCollisionEnter(Collision collision)
    {
        HandleEnter(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        HandleExit(collision.gameObject);
    }

    // เผื่อกรณีเปลี่ยน collider เป็น Is Trigger
    private void OnTriggerEnter(Collider other)
    {
        HandleEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        HandleExit(other.gameObject);
    }

    private void HandleEnter(GameObject other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player == null || player.IsDead)
            return;

        if (Time.time - lastHitTime < hitCooldown)
            return;

        lastHitTime = Time.time;

        if (treeRenderer != null)
            treeRenderer.material.color = hitColor;

        player.HP -= damage;

        if (UIManager.Instance != null)
            UIManager.Instance.ShowNotiText($"Hurt -{damage}");
    }

    private void HandleExit(GameObject other)
    {
        if (other.GetComponentInParent<Player>() == null)
            return;

        if (treeRenderer != null)
            treeRenderer.material.color = originalColor;
    }
}

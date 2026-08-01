using UnityEngine;

public class TreeObstacle : MonoBehaviour
{
    [SerializeField]
    private Color hitColor = Color.red;

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private Renderer treeRenderer;

    private Color originalColor;

    void Start()
    {
        if (treeRenderer == null)
            treeRenderer = GetComponentInChildren<Renderer>();

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
        if (player == null)
            return;

        treeRenderer.material.color = hitColor;
        player.HP -= damage;
        UIManger.is.ShowNotiText($"Hurt-15\n")
    }

    private void HandleExit(GameObject other)
    {
        if (other.GetComponentInParent<Player>() == null)
            return;

        treeRenderer.material.color = originalColor;
    }
}

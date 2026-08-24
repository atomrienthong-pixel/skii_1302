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
    private bool used;

    void Start()
    {
        if (treeRenderer == null)
            treeRenderer = GetComponentInChildren<Renderer>();

        if (treeRenderer != null)
            originalColor = treeRenderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleEnter(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        HandleExit(collision.gameObject);
    }

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
        if (used)
            return;

        Player player = other.GetComponentInParent<Player>();

        if (player == null || player.IsDead || player.IsFinished)
            return;

        used = true;

        if (treeRenderer != null)
            treeRenderer.material.color = hitColor;

        player.HP -= damage;

        if (AudioManager.instance != null)
            AudioManager.instance.PlayHit();

        if (UIManager.instance != null)
            UIManager.instance.ShowNotiText("Hurt -" + damage);
    }

    private void HandleExit(GameObject other)
    {
        if (other.GetComponentInParent<Player>() == null)
            return;

        if (treeRenderer != null)
            treeRenderer.material.color = originalColor;
    }
}

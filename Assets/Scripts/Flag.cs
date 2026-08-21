using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField]
    private int point = 10;

    private bool taken;

    private void OnTriggerEnter(Collider other)
    {
        if (taken)
            return;

        Player player = other.GetComponentInParent<Player>();

        if (player == null || player.IsDead || player.IsFinished)
            return;

        taken = true;
        player.Point += point;

        if (UIManager.instance != null)
            UIManager.instance.ShowNotiText("Flag +" + point);

        Destroy(gameObject);
    }
}

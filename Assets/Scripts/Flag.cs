using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField]
    private int point = 10;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player == null || player.IsDead)
            return;

        player.Point += point;

        if (UIManger.instance != null)
        {
            UIManger.instance.ShowPoint(player.Point);
            UIManger.instance.ShowNotiText($"Flag +{point}");
        }

        Destroy(gameObject);
    }
}

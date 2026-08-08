using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private int coinValue = 1;

    // กันเก็บซ้ำในเฟรมเดียวกัน ก่อนที่ Destroy จะทำงานจริง
    private bool collected;

    // เหรียญควรเป็น Is Trigger ผู้เล่นจะได้วิ่งทะลุ ไม่ใช่ชนกระเด็น
    private void OnTriggerEnter(Collider other)
    {
        HandlePickup(other.gameObject);
    }

    // เผื่อเผลอไม่ได้ติ๊ก Is Trigger
    private void OnCollisionEnter(Collision collision)
    {
        HandlePickup(collision.gameObject);
    }

    private void HandlePickup(GameObject other)
    {
        if (collected)
            return;

        Player player = other.GetComponentInParent<Player>();
        if (player == null || player.IsDead)
            return;

        collected = true;
        player.Point += coinValue;

        if (UIManger.instance != null)
        {
            UIManger.instance.ShowPoint(player.Point);
            UIManger.instance.ShowNotiText($"Coin +{coinValue}");
        }

        Destroy(gameObject);
    }
}

using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private int coinValue = 1;

    [SerializeField]
    private float spinSpeed = 120f;

    private bool collected;

    private void Update()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandlePickup(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandlePickup(collision.gameObject);
    }

    private void HandlePickup(GameObject other)
    {
        if (collected)
            return;

        Player player = other.GetComponentInParent<Player>();

        if (player == null || player.IsDead || player.IsFinished)
            return;

        collected = true;
        player.Point += coinValue;

        if (AudioManager.instance != null)
            AudioManager.instance.PlayCoin();

        if (UIManager.instance != null)
            UIManager.instance.ShowNotiText("Coin +" + coinValue);

        Destroy(gameObject);
    }
}

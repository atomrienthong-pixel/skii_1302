using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField]
    private string finishMessage = "Finish!";

    private bool alreadyFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyFinished)
            return;

        Player p = other.GetComponentInParent<Player>();

        if (p == null || p.IsDead)
            return;

        alreadyFinished = true;

        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver(finishMessage);
    }
}

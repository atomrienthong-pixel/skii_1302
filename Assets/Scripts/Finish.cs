using UnityEngine;

public class Finish : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();

        if (p == null)
            return;

        if (UIManger.instance != null)
            UIManger.instance.ShowNotiText("Finish!");
    }
}

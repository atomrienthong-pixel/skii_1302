using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float forcePower = 900f;

    [SerializeField]
    private int point;

    [SerializeField]
    private int hp = 3;

    private Rigidbody rb;
    private InputAction moveAction;
    private Vector2 moveValue;
    private bool isDead;
    private bool isFinished;

    public bool IsDead { get { return isDead; } }
    public bool IsFinished { get { return isFinished; } }

    public int Point
    {
        get { return point; }
        set
        {
            point = value;

            if (UIManager.instance != null)
                UIManager.instance.ShowPoint(point);
        }
    }

    public int HP
    {
        get { return hp; }
        set
        {
            if (isDead || isFinished)
                return;

            int damage = hp - value;
            hp = Mathf.Max(0, value);

            if (UIManager.instance != null)
            {
                UIManager.instance.ShowHP(hp);

                if (damage > 0)
                    UIManager.instance.ShowDamage(damage);
            }

            if (hp <= 0)
                Die();
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");

        if (UIManager.instance != null)
        {
            UIManager.instance.ShowHP(hp);
            UIManager.instance.ShowPoint(point);
        }
    }

    void FixedUpdate()
    {
        if (isDead || isFinished)
            return;

        MoveLeftOrRight();
    }

    private void MoveLeftOrRight()
    {
        if (moveAction == null)
            return;

        moveValue = moveAction.ReadValue<Vector2>();
        rb.AddForce(moveValue.x * Vector3.right * forcePower);
    }

    private void Stop()
    {
        if (rb == null)
            return;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Die()
    {
        isDead = true;
        Stop();

        if (AudioManager.instance != null)
            AudioManager.instance.PlayGameOver();

        if (UIManager.instance != null)
            UIManager.instance.ShowGameOver("Game Over\nCoin: " + point);
    }

    public void Finish()
    {
        if (isDead || isFinished)
            return;

        isFinished = true;
        Stop();

        if (AudioManager.instance != null)
            AudioManager.instance.PlayFinish();

        if (UIManager.instance != null)
            UIManager.instance.ShowGameOver("Finish!\nCoin: " + point + "   HP: " + hp);
    }
}

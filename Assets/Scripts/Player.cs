using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    [SerializeField]
    private float forcePower;

    [SerializeField]
    private Rigidbody rb;

    private InputAction moveActopn;
    private Vector2 moveValue;

    [SerializeField]
    private int point;
    public int Point { get { return point; } set { point = value; } }

    [SerializeField]
    private int hp;

    private bool isDead;
    public bool IsDead { get { return isDead; } }

    public int HP
    {
        get { return hp; }
        set
        {
            if (isDead)
                return;

            int damage = hp - value;
            hp = Mathf.Max(0, value);

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowHP(hp);

                if (damage > 0)
                    UIManager.Instance.ShowDamage(damage);
            }

            if (hp <= 0)
                Die();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveActopn = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();

        if (UIManager.Instance != null)
            UIManager.Instance.ShowHP(hp);

        if (UIManger.instance != null)
            UIManger.instance.ShowPoint(point);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        MoveLeftOrRight();
    }

    private void MoveLeftOrRight()
    {
        moveValue = moveActopn.ReadValue<Vector2>();
        rb.AddForce(moveValue.x * Vector3.right * forcePower);
    }

    // เลือดหมด: หยุดตัวผู้เล่นแล้วให้ UI เปิดจอ Restart
    private void Die()
    {
        isDead = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (UIManger.instance != null)
            UIManger.instance.ShowGameOver("Game Over");
    }
}

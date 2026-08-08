using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float forcePower;

    [SerializeField]
    private Rigidbody rb;

    private InputAction moveAction;
    private Vector2 moveValue;

    [SerializeField]
    private int point;
    public int Point { get { return point; } set { point = value; } }

    [SerializeField]
    private int maxHp = 100;

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

            int newHp = Mathf.Clamp(value, 0, maxHp);
            int damage = hp - newHp;
            hp = newHp;

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

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        moveAction = InputSystem.actions != null ? InputSystem.actions.FindAction("Move") : null;

        if (moveAction == null)
            Debug.LogError("Player: หา InputAction ชื่อ \"Move\" ไม่เจอ ผู้เล่นจะบังคับไม่ได้", this);

        hp = Mathf.Clamp(hp, 0, maxHp);

        if (UIManager.Instance != null)
            UIManager.Instance.ShowHP(hp);
    }

    // อ่าน input ที่นี่ (ทุกเฟรม) แต่ไม่ยุ่งกับฟิสิกส์
    void Update()
    {
        if (isDead || moveAction == null)
        {
            moveValue = Vector2.zero;
            return;
        }

        moveValue = moveAction.ReadValue<Vector2>();
    }

    // ออกแรงที่นี่ เพราะ FixedUpdate เดินตามรอบฟิสิกส์ ไม่ผันตาม framerate
    void FixedUpdate()
    {
        if (isDead || rb == null)
            return;

        rb.AddForce(moveValue.x * forcePower * Vector3.right);
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver("Game Over");
    }
}

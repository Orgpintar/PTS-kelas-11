using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    public float kecepatan;
    private Vector2 arahGerak;
    private Rigidbody2D rb;
    public ScoreManager ScoreManager;
    private int skor = 0; 
    [SerializeField] private int damageKeZombie;

    InputAction moveAction;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
    }
    
    void Update()
    {
        Move();
        PlayerAnimation();
    }

    void PlayerAnimation()
    {
        if (arahGerak.x < 0)
        {
            animator.SetInteger("Direction", 3);
        }
        else if (arahGerak.x > 0)
        {
            animator.SetInteger("Direction", 2);
        }
        else if (arahGerak.y > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (arahGerak.y < 0)
        {
            animator.SetInteger("Direction", 0);
        }

        animator.SetBool("IsMoving", arahGerak.sqrMagnitude > 0);
            
    }

    void Move()
    {
        if (moveAction != null)
        {
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            arahGerak = moveValue.normalized;
            rb.linearVelocity = arahGerak * kecepatan;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Coin"))
    {
        Destroy(other.gameObject);

        skor += 1;
        ScoreManager.AddScore();

        Debug.Log("Skor: " + skor);
    }

    else if (other.CompareTag("Enemy"))
    {
        IDamageable zombie =
            other.GetComponentInParent<IDamageable>();

        if (zombie != null)
        {
            zombie.KenaDamage(damageKeZombie);

            Debug.Log(
                "Zombie terkena damage: " + damageKeZombie
            );
        }
        else
        {
            Debug.LogWarning(
                "Enemy belum mengimplementasikan IDamageable!"
            );
        }
    }
}
}

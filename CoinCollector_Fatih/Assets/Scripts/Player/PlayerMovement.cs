using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float kecepatan = 5f;
    private Vector2 arahGerak;
    private Rigidbody2D rb;
    public GameManager gameManager;
    private int skor = 0; 
    [SerializeField] private int damageKeZombie = 10;

    InputAction moveAction;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
    }
    
    void Update()
    {
        Move();
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
    // Saat Player mengambil Coin
    if (other.CompareTag("Coin"))
    {
        Destroy(other.gameObject);

        skor += 1;
        gameManager.AddScore();

        Debug.Log("Skor: " + skor);
    }

    // Saat Player mengenai Enemy
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

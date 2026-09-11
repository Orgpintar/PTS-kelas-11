using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int hp = 1000;
    [SerializeField] private int damageSaatTabrakan = 1;

    public float ms = 2f;
    private Vector2 tujuanPatrol;
    private Vector2 titikAwal;

    [SerializeField] private float radiusPatrol = 3f;

    protected Transform player;

    [Header("State Machine")]
    [SerializeField] private float JarakDeteksi = 6f;
    [SerializeField] private float JarakSerang = 1.5f;
    [SerializeField] private float JedaSerang = 2f;

    // State Now
    private StateZombie state = StateZombie.IDLE;
    private float waktuSerangTerakhir;


    public static event Action<Enemy> OnZombieMati;
    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        PeriksaTransisi();
        
        switch(state)
        {
            case StateZombie.IDLE: PerilakuIdle(); break;
            case StateZombie.PATROL: PerilakuPatrol(); break;
            case StateZombie.CHASE: PerilakuChase(); break;
            case StateZombie.ATTACK: PerilakuAttack(); break;
        }
    }

    public float JarakKePlayer()
    {
        if (player == null) return Mathf.Infinity   ;
        return Vector2.Distance(transform.position, player.position);
    }

    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();

        if (jarak <= JarakSerang)
        {
            state = StateZombie.ATTACK;
        }
        else if (jarak <= JarakDeteksi)
        {
            state = StateZombie.CHASE;
        }
        else
        {
            state = StateZombie.PATROL;
        }  
    }

    public void Kejar()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    public virtual void Serang()
    {
    }

    // Dipanggil otomatis oleh Unity saat collider enemy menabrak collider lain.
    // Karena ada di class induk, SEMUA turunan zombie ikut punya perilaku ini.
    void PerilakuIdle()
    {
    }

    void PerilakuPatrol()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            tujuanPatrol,
            ms * 0.5f * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, tujuanPatrol) < 0.1f)
        {
            PilihTujuanPatrolBaru();
        }
    }
    void PilihTujuanPatrolBaru()
    {
        // DIUBAH: memilih Random milik Unity agar tidak ambigu dengan System.Random.
        Vector2 acak = UnityEngine.Random.insideUnitCircle * radiusPatrol;
        tujuanPatrol = titikAwal + acak;
    }

    void PerilakuChase()
    {
        Kejar();
    }

    void PerilakuAttack()
    {
        if (Time.time >= waktuSerangTerakhir + JedaSerang)
        {
            Serang();
            waktuSerangTerakhir = Time.time;
        }
    }

    public void KenaDamage(int damage)
    {
        hp -= damage;
        Debug.Log("Enemy kena damage: " + damage + ", HP sekarang: " + hp);
        
        if (hp <= 0)
        {
            Mati();
        }
    }

    protected virtual void Mati()
    {
        Debug.Log("Enemy mati!");

        // DITAMBAHKAN: menyiarkan event bahwa enemy ini sudah mati.
        OnZombieMati?.Invoke(this);

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable playerScript = other.GetComponent<IDamageable>();
            if (playerScript != null)
            {
                playerScript.KenaDamage(damageSaatTabrakan);
            }
        }
    }
}

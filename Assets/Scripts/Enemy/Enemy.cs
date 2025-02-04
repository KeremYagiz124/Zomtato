using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    public float health = 100f;  // Düþmanýn caný
    public float maxHealth = 100f;  // Maksimum can
    public float moveSpeed = 3f;  // Düþmanýn hareket hýzý
    public bool isFollowing = true;  // Düþmanýn oyuncuyu takip edip etmeyeceðini kontrol eden bool
    public float damageAmount = 10f;
    public float stopDistance = 2f;  // Düþmanýn oyuncuya yaklaþmamasý gereken mesafe (bu mesafeden daha yakýnsa takip etmeyi býrakacak)
    public int enemyLevel = 1;  // Düþmanýn zorluk seviyesi (1-3 arasýnda bir deðer alacak)

    [Header("References")]
    public Transform player;  // Oyuncu objesini referans alacak
    // Para ve kutu prefab'larý
    public GameObject[] coinsPrefabs;  // 0: en düþük, 1: orta, 2: en yüksek para prefab'larý
    public GameObject boxPrefab;  // Kutu prefab'ý

    private Rigidbody2D rb;  // Düþmanýn Rigidbody2D bileþeni
    private Animator animator;  // Düþmanýn Animator bileþeni

    public float dropRate = 0.2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Eðer player nesnesi atanmadýysa, oyuncu tag'ine sahip objeyi bul
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;  // player referansýný ata
            }
            else
            {
                Debug.LogError("Player object not found! Make sure the player has the 'Player' tag.");
            }
        }
    }

    void Update()
    {
        if (isFollowing && player != null)  // player null deðilse takip et
        {
            // Oyuncu ile düþman arasýndaki mesafeyi hesapla
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Eðer mesafe stopDistance'dan küçükse, düþman takip etmeyi durdurur
            if (distanceToPlayer > stopDistance)
            {
                FollowPlayer();
            }
            else
            {
                rb.velocity = Vector2.zero; // Takip etmeyi durdur, hareket etmeyi býrak
                animator.SetTrigger("Close");
            }
        }
    }
    public void Hit(float damage)
    {
        health -= damage;

        if (health <= 0)
            OnDeath();
    }
    void FollowPlayer()
    {
        // Oyuncunun konumuna doðru hareket et
        Vector2 direction = (player.position - transform.position).normalized;  // Oyuncu ile düþman arasýndaki yön vektörü
        rb.velocity = direction * moveSpeed;  // Hýzý ayarla
    }

    // Düþman ile çarpýþma durumunda yapýlacak iþlemler
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Player'a hasar verme iþlemi
            PlayerController playerController = collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damageAmount);
            }
        }
        if (collider.CompareTag("Projectile"))
        {
            Hit(collider.GetComponent<Projectile2>().getDamage());
            Destroy(collider.gameObject);
            Debug.Log(collider.GetComponent<Projectile2>().getDamage());
        }
    }

    // Düþman öldüðünde para ve kutu düþürme iþlemi
    public void DropLoot()
    {
        // Düþman öldüðünde rastgele bir loot (para/kutu) seç

        // Daha zor düþmanlardan daha iyi loot düþmesi için
        for (float i = 0; i < dropRate; i += 1)
        {
            float dropChance = Random.value;  // Rastgele bir deðer (0-1 arasýnda)
            if (enemyLevel == 1)  // Kolay düþman
            {
                if (dropChance < 0.3f)  // %30 ihtimalle para düþer
                {
                    DropCoin(0);  // En düþük seviyede para
                }
                else if (dropChance < 0.5f)  // %20 ihtimalle kutu düþer
                {
                    DropBox();
                }
            }
            else if (enemyLevel == 2)  // Orta seviye düþman
            {
                if (dropChance < 0.5f)  // %50 ihtimalle para düþer
                {
                    DropCoin(1);  // Orta seviyede para
                }
                else if (dropChance < 0.7f)  // %20 ihtimalle kutu düþer
                {
                    DropBox();
                }
            }
            else if (enemyLevel == 3)  // Zor düþman
            {
                if (dropChance < 0.7f)  // %70 ihtimalle para düþer
                {
                    DropCoin(2);  // En yüksek seviyede para
                }
                else if (dropChance < 0.9f)  // %20 ihtimalle kutu düþer
                {
                    DropBox();
                }
            }
        }
    }

    // Para düþürme fonksiyonu
    void DropCoin(int coinIndex)
    {
        Instantiate(coinsPrefabs[coinIndex], transform.position, Quaternion.identity);
    }

    // Kutu düþürme fonksiyonu
    void DropBox()
    {
        Instantiate(boxPrefab, transform.position, Quaternion.identity);
    }

    // Düþman öldüðünde para ve kutu býrak
    void OnDeath()
    {
        DropLoot();  // Para ve kutu düþür
        Destroy(gameObject);
    }

    // Düþman öldüðünde OnDeath fonksiyonunu çaðýr
    public void Die()
    {
        animator.SetTrigger("Die");  // Ölüm animasyonu tetikle
        rb.velocity = Vector2.zero;  // Karakterin hareketini durdur

        OnDeath();  // Loot düþürme iþlemi

        // Öldükten sonra düþmaný yok et
        Destroy(gameObject, 1f);  // 1 saniye sonra yok et
    }
}

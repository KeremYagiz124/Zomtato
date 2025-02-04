using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    public float health = 100f;  // D��man�n can�
    public float maxHealth = 100f;  // Maksimum can
    public float moveSpeed = 3f;  // D��man�n hareket h�z�
    public bool isFollowing = true;  // D��man�n oyuncuyu takip edip etmeyece�ini kontrol eden bool
    public float damageAmount = 10f;
    public float stopDistance = 2f;  // D��man�n oyuncuya yakla�mamas� gereken mesafe (bu mesafeden daha yak�nsa takip etmeyi b�rakacak)
    public int enemyLevel = 1;  // D��man�n zorluk seviyesi (1-3 aras�nda bir de�er alacak)

    [Header("References")]
    public Transform player;  // Oyuncu objesini referans alacak
    // Para ve kutu prefab'lar�
    public GameObject[] coinsPrefabs;  // 0: en d���k, 1: orta, 2: en y�ksek para prefab'lar�
    public GameObject boxPrefab;  // Kutu prefab'�

    private Rigidbody2D rb;  // D��man�n Rigidbody2D bile�eni
    private Animator animator;  // D��man�n Animator bile�eni

    public float dropRate = 0.2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // E�er player nesnesi atanmad�ysa, oyuncu tag'ine sahip objeyi bul
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;  // player referans�n� ata
            }
            else
            {
                Debug.LogError("Player object not found! Make sure the player has the 'Player' tag.");
            }
        }
    }

    void Update()
    {
        if (isFollowing && player != null)  // player null de�ilse takip et
        {
            // Oyuncu ile d��man aras�ndaki mesafeyi hesapla
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // E�er mesafe stopDistance'dan k���kse, d��man takip etmeyi durdurur
            if (distanceToPlayer > stopDistance)
            {
                FollowPlayer();
            }
            else
            {
                rb.velocity = Vector2.zero; // Takip etmeyi durdur, hareket etmeyi b�rak
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
        // Oyuncunun konumuna do�ru hareket et
        Vector2 direction = (player.position - transform.position).normalized;  // Oyuncu ile d��man aras�ndaki y�n vekt�r�
        rb.velocity = direction * moveSpeed;  // H�z� ayarla
    }

    // D��man ile �arp��ma durumunda yap�lacak i�lemler
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Player'a hasar verme i�lemi
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

    // D��man �ld���nde para ve kutu d���rme i�lemi
    public void DropLoot()
    {
        // D��man �ld���nde rastgele bir loot (para/kutu) se�

        // Daha zor d��manlardan daha iyi loot d��mesi i�in
        for (float i = 0; i < dropRate; i += 1)
        {
            float dropChance = Random.value;  // Rastgele bir de�er (0-1 aras�nda)
            if (enemyLevel == 1)  // Kolay d��man
            {
                if (dropChance < 0.3f)  // %30 ihtimalle para d��er
                {
                    DropCoin(0);  // En d���k seviyede para
                }
                else if (dropChance < 0.5f)  // %20 ihtimalle kutu d��er
                {
                    DropBox();
                }
            }
            else if (enemyLevel == 2)  // Orta seviye d��man
            {
                if (dropChance < 0.5f)  // %50 ihtimalle para d��er
                {
                    DropCoin(1);  // Orta seviyede para
                }
                else if (dropChance < 0.7f)  // %20 ihtimalle kutu d��er
                {
                    DropBox();
                }
            }
            else if (enemyLevel == 3)  // Zor d��man
            {
                if (dropChance < 0.7f)  // %70 ihtimalle para d��er
                {
                    DropCoin(2);  // En y�ksek seviyede para
                }
                else if (dropChance < 0.9f)  // %20 ihtimalle kutu d��er
                {
                    DropBox();
                }
            }
        }
    }

    // Para d���rme fonksiyonu
    void DropCoin(int coinIndex)
    {
        Instantiate(coinsPrefabs[coinIndex], transform.position, Quaternion.identity);
    }

    // Kutu d���rme fonksiyonu
    void DropBox()
    {
        Instantiate(boxPrefab, transform.position, Quaternion.identity);
    }

    // D��man �ld���nde para ve kutu b�rak
    void OnDeath()
    {
        DropLoot();  // Para ve kutu d���r
        Destroy(gameObject);
    }

    // D��man �ld���nde OnDeath fonksiyonunu �a��r
    public void Die()
    {
        animator.SetTrigger("Die");  // �l�m animasyonu tetikle
        rb.velocity = Vector2.zero;  // Karakterin hareketini durdur

        OnDeath();  // Loot d���rme i�lemi

        // �ld�kten sonra d��man� yok et
        Destroy(gameObject, 1f);  // 1 saniye sonra yok et
    }
}

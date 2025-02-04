using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [Header("Health Properties")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Movement Properties")]
    public float moveSpeed = 3f;
    public float stopDistance = 2f;
    public Transform player;

    [Header("Loot Properties")]
    public GameObject[] coinsPrefabs; // 0: düþük, 1: orta, 2: yüksek seviye para prefab'larý
    public GameObject boxPrefab;

    [Header("Other References")]
    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        // Saðlýk baþlatma
        currentHealth = maxHealth;

        // Rigidbody ve Animator referanslarýný al
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Oyuncuyu bul
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > stopDistance)
            {
                FollowPlayer();
            }
            else
            {
                rb.velocity = Vector2.zero; // Hareket etmeyi býrak
                animator?.SetTrigger("Close"); // Yakýn animasyon tetikle
            }

            // Yüz dönüþü
            bool playerToTheRight = player.position.x < transform.position.x;
            transform.localScale = new Vector2(playerToTheRight ? 1 : -1, 1);
        }
    }

    private void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;
        animator?.SetTrigger("Hit"); // Hasar animasyonunu tetikle

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator?.SetTrigger("Die"); // Ölüm animasyonunu tetikle
        rb.velocity = Vector2.zero; // Hareketi durdur

        DropLoot(); // Loot býrak

        Destroy(gameObject, 1f); // 1 saniye sonra düþmaný yok et
    }

    private void DropLoot()
    {
        float dropChance = Random.value;

        if (dropChance < 0.3f)
        {
            DropCoin(0); // Düþük seviye para
        }
        else if (dropChance < 0.5f)
        {
            DropCoin(1); // Orta seviye para
        }
        else if (dropChance < 0.7f)
        {
            DropCoin(2); // Yüksek seviye para
        }

        if (dropChance < 0.2f)
        {
            DropBox();
        }
    }

    private void DropCoin(int coinIndex)
    {
        if (coinIndex >= 0 && coinIndex < coinsPrefabs.Length)
        {
            Instantiate(coinsPrefabs[coinIndex], transform.position, Quaternion.identity);
        }
    }

    private void DropBox()
    {
        Instantiate(boxPrefab, transform.position, Quaternion.identity);
    }
}

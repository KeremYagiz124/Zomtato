using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    // Hareket Deðiþkenleri
    public float moveSpeed = 5f;  // Karakterin hareket hýzý

    private Rigidbody2D rb;        // Karakterin Rigidbody2D bileþeni
    private Animator animator;     // Karakterin Animator bileþeni
    private Vector2 moveInput;    // Hareket giriþi (klavye veya joystick)
    private Vector2 moveVelocity; // Karakterin hareket hýzý ve yönü
    private bool canMove = true;  // Karakter hareket edebiliyor mu?
    [SerializeField] public bool unDead;
    GameObject HealthUI;
    int coin;
    public int Coin
    {
        get { return coin; }    // Getter, '_isActive' deðerini döndürür
        set { coin = value; }   // Setter, '_isActive' deðerini atar
    }

    // Can Deðiþkenleri
    public float maxHealth = 100f;  // Maksimum can
    [SerializeField] protected float currentHealth;    // Mevcut can

    // Mýknatýs aktif olduðunda paralarý uzaktan toplamak için
    private bool isMagnetActive = false;

    public float magnetRange = 5f; // Mýknatýsýn etkili olduðu mesafe

    private EnemySpawner enemySpawner; // EnemySpawner referansý

    // Baþlangýçta yapýlan iþlemler
    void Start()
    {
        HealthUI = GameObject.FindWithTag("HealthUI");
        coin = 0;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        unDead = false;

        // EnemySpawner objesini sahnede bul
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update metodu: Hareket ve Animasyon güncellemelerini kontrol eder
    void Update()
    {
        // Eðer can sýfýrsa, ölümü tetikle
        if (currentHealth <= 0f)
        {
            Die();
            return;
        }

        if (canMove)
        {
            // Hareket giriþlerini al
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            // Hareket hýzýný belirle
            moveVelocity = moveInput.normalized * moveSpeed;

            // Animasyonlarý güncelle
            UpdateAnimations();

            // Karakterin yönünü döndür
            FlipCharacter();

            if (isMagnetActive)
            {
                CollectCoinsWithMagnet();
            }
        }
    }
    // Karakterin hareketini engelle
    public void DisableMovement()
    {
        canMove = false;
    }

    // Karakterin hareketini tekrar etkinleþtir
    public void EnableMovement()
    {
        canMove = true;
    }

    // FixedUpdate metodu: Karakterin fiziksel hareketini yapar
    void FixedUpdate()
    {
        if (currentHealth > 0f)
        {
            // Karakteri fiziksel olarak hareket ettir
            rb.velocity = moveVelocity;
        }
        else
        {
            rb.velocity = Vector2.zero; // Can sýfýr olduðunda hareketi durdur
        }
    }

    // Animasyonlarý güncelleyen metod
    void UpdateAnimations()
    {
        // Hareket ediyor mu kontrol et
        bool isMoving = moveInput.x != 0 || moveInput.y != 0;

        // Animasyonlarý güncelle
        animator.SetBool("isMoving", isMoving);
    }

    // Hasar alma fonksiyonu
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;  // Caný azalt
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        if (currentHealth <= 0f)  // Eðer can sýfýra düþerse
        {
            currentHealth = 0f;
            Die();  // Ölüm fonksiyonunu tetikle
        }
    }

    // Ölüm fonksiyonu
    void Die()
    {
        animator.SetTrigger("Die");  // Ölüm animasyonunu tetikle
        rb.velocity = Vector2.zero;  // Karakterin hareketini durdur

        // EnemySpawner'daki düþman doðurmayý durdur
        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }

        // Ekstra: Karakteri öldükten sonra ekrana bir þeyler göstermek isteyebilirsiniz
        // Örneðin, bir "Game Over" ekraný açýlabilir veya baþka bir iþlem yapýlabilir.
    }

    // Düþmanlarla etkileþimde hasar almak için bir örnek
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.damageAmount);  // Düþmandan hasar al
                HealthUI.GetComponent<Animator>().SetTrigger("Damaged");
            }
        }
        //if (collision.CompareTag("Coin"))
        //{
        //    Destroy(collision.gameObject, 5f);
        //    coin += collision.GetComponent<Coins>().GetValue();
        //    Debug.Log(coin);
        //}
    }

    // Karakterin yönünü saða veya sola döndürme fonksiyonu
    void FlipCharacter()
    {
        // Eðer hareket saða doðruysa
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Sað yön (1x, 1y, 1z)
        }
        // Eðer hareket sola doðruysa
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // Sol yön (-1x, 1y, 1z)
        }
    }

    // Mýknatýs ile yakýnlardaki paralarý toplama
    void CollectCoinsWithMagnet()
    {
        Collider2D[] coins = Physics2D.OverlapCircleAll(transform.position, magnetRange);
        foreach (Collider2D c in coins)
        {
            if (c.CompareTag("Coin"))
            {
                c.GetComponent<Coins>().Fading();

                // Hedef konumuna doðru hareket et
                Vector3 direction = (transform.position - c.transform.position).normalized; // Hedefe doðru yön
                c.transform.position += direction * 5 * Time.deltaTime; // Hareket

            }
        }
    }

    // Mýknatýs aktif olduktan sonra paralarý toplama iþlemi
    public IEnumerator ActivateMagnet(float duration)
    {
        isMagnetActive = true;
        yield return new WaitForSeconds(duration);
        isMagnetActive = false;
    }

    public void setCurrentHealth(float health)
    {
        currentHealth = health;
    }
    public float getMaxHealth()
    {
        return maxHealth;
    }
    public float getCurrentHealth()
    {
        return currentHealth;
    }
    public int GetCoins()
    {
        return coin;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    // Hareket De�i�kenleri
    public float moveSpeed = 5f;  // Karakterin hareket h�z�

    private Rigidbody2D rb;        // Karakterin Rigidbody2D bile�eni
    private Animator animator;     // Karakterin Animator bile�eni
    private Vector2 moveInput;    // Hareket giri�i (klavye veya joystick)
    private Vector2 moveVelocity; // Karakterin hareket h�z� ve y�n�
    private bool canMove = true;  // Karakter hareket edebiliyor mu?
    [SerializeField] public bool unDead;
    GameObject HealthUI;
    int coin;
    public int Coin
    {
        get { return coin; }    // Getter, '_isActive' de�erini d�nd�r�r
        set { coin = value; }   // Setter, '_isActive' de�erini atar
    }

    // Can De�i�kenleri
    public float maxHealth = 100f;  // Maksimum can
    [SerializeField] protected float currentHealth;    // Mevcut can

    // M�knat�s aktif oldu�unda paralar� uzaktan toplamak i�in
    private bool isMagnetActive = false;

    public float magnetRange = 5f; // M�knat�s�n etkili oldu�u mesafe

    private EnemySpawner enemySpawner; // EnemySpawner referans�

    // Ba�lang��ta yap�lan i�lemler
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

    // Update metodu: Hareket ve Animasyon g�ncellemelerini kontrol eder
    void Update()
    {
        // E�er can s�f�rsa, �l�m� tetikle
        if (currentHealth <= 0f)
        {
            Die();
            return;
        }

        if (canMove)
        {
            // Hareket giri�lerini al
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            // Hareket h�z�n� belirle
            moveVelocity = moveInput.normalized * moveSpeed;

            // Animasyonlar� g�ncelle
            UpdateAnimations();

            // Karakterin y�n�n� d�nd�r
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

    // Karakterin hareketini tekrar etkinle�tir
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
            rb.velocity = Vector2.zero; // Can s�f�r oldu�unda hareketi durdur
        }
    }

    // Animasyonlar� g�ncelleyen metod
    void UpdateAnimations()
    {
        // Hareket ediyor mu kontrol et
        bool isMoving = moveInput.x != 0 || moveInput.y != 0;

        // Animasyonlar� g�ncelle
        animator.SetBool("isMoving", isMoving);
    }

    // Hasar alma fonksiyonu
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;  // Can� azalt
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        if (currentHealth <= 0f)  // E�er can s�f�ra d��erse
        {
            currentHealth = 0f;
            Die();  // �l�m fonksiyonunu tetikle
        }
    }

    // �l�m fonksiyonu
    void Die()
    {
        animator.SetTrigger("Die");  // �l�m animasyonunu tetikle
        rb.velocity = Vector2.zero;  // Karakterin hareketini durdur

        // EnemySpawner'daki d��man do�urmay� durdur
        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
        }

        // Ekstra: Karakteri �ld�kten sonra ekrana bir �eyler g�stermek isteyebilirsiniz
        // �rne�in, bir "Game Over" ekran� a��labilir veya ba�ka bir i�lem yap�labilir.
    }

    // D��manlarla etkile�imde hasar almak i�in bir �rnek
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                TakeDamage(enemy.damageAmount);  // D��mandan hasar al
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

    // Karakterin y�n�n� sa�a veya sola d�nd�rme fonksiyonu
    void FlipCharacter()
    {
        // E�er hareket sa�a do�ruysa
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Sa� y�n (1x, 1y, 1z)
        }
        // E�er hareket sola do�ruysa
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // Sol y�n (-1x, 1y, 1z)
        }
    }

    // M�knat�s ile yak�nlardaki paralar� toplama
    void CollectCoinsWithMagnet()
    {
        Collider2D[] coins = Physics2D.OverlapCircleAll(transform.position, magnetRange);
        foreach (Collider2D c in coins)
        {
            if (c.CompareTag("Coin"))
            {
                c.GetComponent<Coins>().Fading();

                // Hedef konumuna do�ru hareket et
                Vector3 direction = (transform.position - c.transform.position).normalized; // Hedefe do�ru y�n
                c.transform.position += direction * 5 * Time.deltaTime; // Hareket

            }
        }
    }

    // M�knat�s aktif olduktan sonra paralar� toplama i�lemi
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

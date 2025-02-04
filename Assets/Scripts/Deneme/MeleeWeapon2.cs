using System.Collections;
using UnityEngine;

public class MeleeWeapon2 : Weapon2
{
    [SerializeField] private float attackSpeed = 5f; // Sald�r� h�z�
    [SerializeField] private float attackCooldown = 1f; // Sald�r� aras�nda ge�en s�re

    private Vector3 originalPosition; // Silah�n ba�lang�� pozisyonu
    private bool isAttacking = false; // Sald�r�n�n devam etti�ini kontrol eden bayrak
    //private Transform closestEnemy; // En yak�n d��man
    private Enemy closestEnemy2; // En yak�n d��man
    private Transform character; // Karakter referans�

    [SerializeField] float returnSpeed = 3f; // Geri d�n�� h�z� (ayarlanabilir)

    private float lastAttackTime;

    public GameObject wui;
    WeaponUI wuis;


    private void Start()
    {
        wui = GameObject.Find("Weapons");
        wuis = wui.GetComponent<WeaponUI>();
        character = GameObject.FindWithTag("Player").transform; // Karakteri bul (Player tag'ini kullanarak)
        originalPosition = character.position + weaponOffset; // Silah�n ba�lang�� pozisyonu
        transform.position = originalPosition; // Silah� ba�lang�� konumuna yerle�tir
        weaponType = WeaponType2.Melee;

        int index = -1;
        // weapons dizisini kontrol ederek do�ru silah� bul
        for (int i = 0; i < wuis.weapons.Length; i++)
        {
            if (wuis.weapons[i] == gameObject)  // Silah�n kendisini e�le�tiriyoruz
            {
                index = i;
                break;
            }
        }

        // Silah konumlar�n� ayarl�yoruz
        switch (index)
        {
            case 0:
                weaponOffset = wuis.v0;
                break;
            case 1:
                weaponOffset = wuis.v1;
                break;
            case 2:
                weaponOffset = wuis.v2;
                break;
            case 3:
                weaponOffset = wuis.v3;
                break;
        }

    }

    private void Update()
    {
        // Karakterin etraf�ndaki silah pozisyonunu s�rekli olarak g�ncel tutuyoruz
        originalPosition = character.position + weaponOffset; 
        transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * attackSpeed); 

        // En yak�n d��man� bulma i�lemi
        FindClosestEnemy();

        // En yak�n d��man varsa sald�r�ya ba�la
        if (closestEnemy != null && !isAttacking && Time.time - lastAttackTime > attackCooldown)
        {
            if (Vector2.Distance(transform.position, closestEnemy.position) <= range)
            {
                AimAtEnemy(closestEnemy);
                StartCoroutine(Attack(closestEnemy)); // Sald�r� ba�lat
                lastAttackTime = Time.time;
            }
        }
    }

    public override void Fire(Vector3 targetPosition)
    {
        // Melee silah�n Fire metoduna gerek yok, ��nk� Update ile otomatik sald�r� yap�lacak
    }

    private void FindClosestEnemy()
    {
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in FindObjectsOfType<Enemy>()) // Enemy s�n�f�n�z� kendi oyununuzda uygun �ekilde de�i�tirin
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
    }

    private IEnumerator Attack(Transform enemy)
    {
        if (enemy == null)
        {
            isAttacking = false;
            yield break; // D��man yoksa sald�r�y� sonland�r
        }

        isAttacking = true; // Sald�r� ba�l�yor

        // D��mana do�ru y�nelmek i�in d�nd�rme
        Vector3 directionToEnemy = (enemy.position - transform.position).normalized;
        Vector3 characterVelocity = character.GetComponent<Rigidbody2D>().velocity;
        directionToEnemy = Vector3.Lerp(directionToEnemy, characterVelocity, 0.5f);
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToEnemy);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, attackSpeed * Time.deltaTime);

        float characterSpeed = character.GetComponent<Rigidbody2D>().velocity.magnitude;
        float adjustedReturnSpeed = returnSpeed * (1 + characterSpeed); // Karakterin h�z� artt�k�a geri d�n�� h�z� da artar

        // D��mana do�ru hareket et
        while (enemy != null && Vector3.Distance(transform.position, enemy.position) > 0.1f)
        {
            if (Vector3.Distance(transform.position, enemy.position) > range)
                break;
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, enemy.position, attackSpeed * Time.deltaTime);
                yield return null;
            }
        }


        while (Vector3.Distance(transform.position, originalPosition) > 0f)
        {
            // D�n�� s�ras�nda da d�nd�rme yaparak daha do�al bir ge�i� sa�la
            targetRotation = Quaternion.LookRotation(Vector3.forward, (originalPosition - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, attackSpeed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, originalPosition, adjustedReturnSpeed * Time.deltaTime);
            yield return null;

        }

        isAttacking = false; // Sald�r� bitti, tekrar sald�r�labilir
    }

    public override void Upgrade()
    {
        Debug.Log("Melee weapon upgraded.");
        base.Upgrade();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //var enemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Hit(damage);
            Debug.Log(damage);
        }
    }
}

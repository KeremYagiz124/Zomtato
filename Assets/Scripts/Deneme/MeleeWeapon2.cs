using System.Collections;
using UnityEngine;

public class MeleeWeapon2 : Weapon2
{
    [SerializeField] private float attackSpeed = 5f; // Saldýrý hýzý
    [SerializeField] private float attackCooldown = 1f; // Saldýrý arasýnda geçen süre

    private Vector3 originalPosition; // Silahýn baþlangýç pozisyonu
    private bool isAttacking = false; // Saldýrýnýn devam ettiðini kontrol eden bayrak
    //private Transform closestEnemy; // En yakýn düþman
    private Enemy closestEnemy2; // En yakýn düþman
    private Transform character; // Karakter referansý

    [SerializeField] float returnSpeed = 3f; // Geri dönüþ hýzý (ayarlanabilir)

    private float lastAttackTime;

    public GameObject wui;
    WeaponUI wuis;


    private void Start()
    {
        wui = GameObject.Find("Weapons");
        wuis = wui.GetComponent<WeaponUI>();
        character = GameObject.FindWithTag("Player").transform; // Karakteri bul (Player tag'ini kullanarak)
        originalPosition = character.position + weaponOffset; // Silahýn baþlangýç pozisyonu
        transform.position = originalPosition; // Silahý baþlangýç konumuna yerleþtir
        weaponType = WeaponType2.Melee;

        int index = -1;
        // weapons dizisini kontrol ederek doðru silahý bul
        for (int i = 0; i < wuis.weapons.Length; i++)
        {
            if (wuis.weapons[i] == gameObject)  // Silahýn kendisini eþleþtiriyoruz
            {
                index = i;
                break;
            }
        }

        // Silah konumlarýný ayarlýyoruz
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
        // Karakterin etrafýndaki silah pozisyonunu sürekli olarak güncel tutuyoruz
        originalPosition = character.position + weaponOffset; 
        transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * attackSpeed); 

        // En yakýn düþmaný bulma iþlemi
        FindClosestEnemy();

        // En yakýn düþman varsa saldýrýya baþla
        if (closestEnemy != null && !isAttacking && Time.time - lastAttackTime > attackCooldown)
        {
            if (Vector2.Distance(transform.position, closestEnemy.position) <= range)
            {
                AimAtEnemy(closestEnemy);
                StartCoroutine(Attack(closestEnemy)); // Saldýrý baþlat
                lastAttackTime = Time.time;
            }
        }
    }

    public override void Fire(Vector3 targetPosition)
    {
        // Melee silahýn Fire metoduna gerek yok, çünkü Update ile otomatik saldýrý yapýlacak
    }

    private void FindClosestEnemy()
    {
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in FindObjectsOfType<Enemy>()) // Enemy sýnýfýnýzý kendi oyununuzda uygun þekilde deðiþtirin
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
            yield break; // Düþman yoksa saldýrýyý sonlandýr
        }

        isAttacking = true; // Saldýrý baþlýyor

        // Düþmana doðru yönelmek için döndürme
        Vector3 directionToEnemy = (enemy.position - transform.position).normalized;
        Vector3 characterVelocity = character.GetComponent<Rigidbody2D>().velocity;
        directionToEnemy = Vector3.Lerp(directionToEnemy, characterVelocity, 0.5f);
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToEnemy);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, attackSpeed * Time.deltaTime);

        float characterSpeed = character.GetComponent<Rigidbody2D>().velocity.magnitude;
        float adjustedReturnSpeed = returnSpeed * (1 + characterSpeed); // Karakterin hýzý arttýkça geri dönüþ hýzý da artar

        // Düþmana doðru hareket et
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
            // Dönüþ sýrasýnda da döndürme yaparak daha doðal bir geçiþ saðla
            targetRotation = Quaternion.LookRotation(Vector3.forward, (originalPosition - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, attackSpeed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, originalPosition, adjustedReturnSpeed * Time.deltaTime);
            yield return null;

        }

        isAttacking = false; // Saldýrý bitti, tekrar saldýrýlabilir
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

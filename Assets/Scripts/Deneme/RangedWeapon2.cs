using Goldmetal.UndeadSurvivor;
using UnityEngine;

public class RangedWeapon2 : Weapon2
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float fireCooldown = 1f; // Ate� etme s�resi

    private float nextFireTime = 0f; // Sonraki ate� zaman�

    public GameObject wui;
    WeaponUI wuis;


    private void Start()
    {
        weaponType = WeaponType2.Ranged;

        wui = GameObject.Find("Weapons");
        wuis = wui.GetComponent<WeaponUI>();

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
        FindClosestEnemy();
        if (closestEnemy != null)
            if (Vector2.Distance(transform.position, closestEnemy.transform.position) <= range)
                AimAtEnemy(closestEnemy);

        // Ate� etme zaman� kontrol�
        if (getClosestEnemy() != null && Time.time >= nextFireTime)
        {
            Fire(getClosestEnemy().position);
            nextFireTime = Time.time + fireRate; // Ate� h�z�na g�re sonraki ate� zaman�n� ayarla
        }
    }

    // Ate� etme metodu
    public override void Fire(Vector3 targetPosition)
    {
        ShootProjectile(targetPosition);
    }

    // Mermi f�rlatma
    private void ShootProjectile(Vector3 targetPosition)
    {
        // Mermi olu�tur
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Hedefe do�ru y�n� hesapla
        Vector2 direction = (targetPosition - projectileSpawnPoint.position).normalized;

        // Mermiyi hedefe y�nlendir
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 10f; // Mermiyi h�zland�r
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody2D component!");
        }

        // Merminin rotasyonunu ayarla (iste�e ba�l�)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Y�kseltme fonksiyonu
    public override void Upgrade()
    {
        base.Upgrade();
        Debug.Log("Ranged Weapon Upgraded");
    }
    public float GetDamage()
    {
        return damage;
    }
}

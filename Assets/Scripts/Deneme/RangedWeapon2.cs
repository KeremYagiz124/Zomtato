using Goldmetal.UndeadSurvivor;
using UnityEngine;

public class RangedWeapon2 : Weapon2
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float fireCooldown = 1f; // Ateþ etme süresi

    private float nextFireTime = 0f; // Sonraki ateþ zamaný

    public GameObject wui;
    WeaponUI wuis;


    private void Start()
    {
        weaponType = WeaponType2.Ranged;

        wui = GameObject.Find("Weapons");
        wuis = wui.GetComponent<WeaponUI>();

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
        FindClosestEnemy();
        if (closestEnemy != null)
            if (Vector2.Distance(transform.position, closestEnemy.transform.position) <= range)
                AimAtEnemy(closestEnemy);

        // Ateþ etme zamaný kontrolü
        if (getClosestEnemy() != null && Time.time >= nextFireTime)
        {
            Fire(getClosestEnemy().position);
            nextFireTime = Time.time + fireRate; // Ateþ hýzýna göre sonraki ateþ zamanýný ayarla
        }
    }

    // Ateþ etme metodu
    public override void Fire(Vector3 targetPosition)
    {
        ShootProjectile(targetPosition);
    }

    // Mermi fýrlatma
    private void ShootProjectile(Vector3 targetPosition)
    {
        // Mermi oluþtur
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Hedefe doðru yönü hesapla
        Vector2 direction = (targetPosition - projectileSpawnPoint.position).normalized;

        // Mermiyi hedefe yönlendir
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 10f; // Mermiyi hýzlandýr
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody2D component!");
        }

        // Merminin rotasyonunu ayarla (isteðe baðlý)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Yükseltme fonksiyonu
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

using System.Collections;
using UnityEngine;

public enum WeaponType2
{
    Melee,
    Ranged
}

public abstract class Weapon2 : MonoBehaviour
{
    public WeaponType2 weaponType;
    public float damage;  // Silahýn hasarý
    public float fireRate;  // Ateþ hýzý
    public float fireDistance;  // Ateþ hýzý
    public float range;  // Menzil
    public float price;  // Fiyat
    public Sprite weaponSprite;  // Silahýn görseli (Sprite)
    public Transform weaponTip;  // Silahýn uç noktasýný temsil eden Transform
    protected Transform closestEnemy;
    public LayerMask enemyLayer;  // Düþmanlarýn bulunduðu layer

    protected Vector3 weaponOffset; // Silahýn karakterin etrafýndaki ofset pozisyonu

    int weaponIndex;

    private void Start()
    {

        //switch (name)
        //{
        //case "Weapon 0(Clone)":
        //    damage = 30;
        //    break;
        //case "Weapon 1(Clone)":
        //    damage = 40;
        //    break;
        //case "Weapon 2(Clone)":
        //    damage = 50;
        //    break;
        //case "Weapon 3(Clone)":
        //    damage = 70;
        //    break;
        //case "Weapon 4(Clone)":
        //    damage = 80;
        //    break;
        //case "Weapon 5(Clone)":
        //    damage = 100;
        //    break;
        //}

        //Debug.Log(name);

        //if (name == "Weapon 0(Clone)")
        //    damage = 30;
        //if (name == "Weapon 1(Clone)")
        //    damage = 40;
        //if (name == "Weapon 2(Clone)")
        //    damage = 50;
        //if (name == "Weapon 3(Clone)")
        //    damage = 70;
        //if (name == "Weapon 4(Clone)")
        //    damage = 80;
        //if (name == "Weapon 5(Clone)")
        //    Debug.Log("deneme");



    }

    // Silahýn ateþ etme fonksiyonu (Soyut metod, alt sýnýflarda uygulanacak)
    public abstract void Fire(Vector3 targetPosition);
    // Silah yükseltme fonksiyonu
    public virtual void Upgrade()
    {
        damage += 10f;  // Hasar artýrma
        fireRate += 1f;  // Ateþ hýzýný artýrma
        range *= 2f;  // Menzili artýrma
        Debug.Log($"Weapon upgraded: Damage={damage}, FireRate={fireRate}, Range={range}");
    }

    public int WeaponIndex
    {
        get { return weaponIndex; }
        set { weaponIndex = value; }
    }

    // Silahýn uç noktasýný en yakýn düþmana doðru döndürme
    public void UpdateWeaponOrientation()
    {
        // En yakýn düþmaný bul
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        if (enemies.Length > 0)
        {
            // En yakýn düþmaný bul
            FindClosestEnemy();

            if (closestEnemy != null)
            {
                // Silahýn uç kýsmýný en yakýn düþmana doðru döndür
                Vector3 directionToEnemy = closestEnemy.position - weaponTip.position;
                Quaternion rotation = Quaternion.LookRotation(directionToEnemy);
                weaponTip.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0); // Y ekseninde döndür
            }
        }
    }

    // En yakýn düþmaný bulma fonksiyonu
    protected void FindClosestEnemy()
    {
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= fireDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }
    }
    protected void AimAtEnemy(Transform closestEnemy)
    {
        if (closestEnemy == null) return;

        Vector3 direction = closestEnemy.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public Transform getClosestEnemy()
    {
        return closestEnemy;
    }

}
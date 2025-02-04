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
    public float damage;  // Silah�n hasar�
    public float fireRate;  // Ate� h�z�
    public float fireDistance;  // Ate� h�z�
    public float range;  // Menzil
    public float price;  // Fiyat
    public Sprite weaponSprite;  // Silah�n g�rseli (Sprite)
    public Transform weaponTip;  // Silah�n u� noktas�n� temsil eden Transform
    protected Transform closestEnemy;
    public LayerMask enemyLayer;  // D��manlar�n bulundu�u layer

    protected Vector3 weaponOffset; // Silah�n karakterin etraf�ndaki ofset pozisyonu

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

    // Silah�n ate� etme fonksiyonu (Soyut metod, alt s�n�flarda uygulanacak)
    public abstract void Fire(Vector3 targetPosition);
    // Silah y�kseltme fonksiyonu
    public virtual void Upgrade()
    {
        damage += 10f;  // Hasar art�rma
        fireRate += 1f;  // Ate� h�z�n� art�rma
        range *= 2f;  // Menzili art�rma
        Debug.Log($"Weapon upgraded: Damage={damage}, FireRate={fireRate}, Range={range}");
    }

    public int WeaponIndex
    {
        get { return weaponIndex; }
        set { weaponIndex = value; }
    }

    // Silah�n u� noktas�n� en yak�n d��mana do�ru d�nd�rme
    public void UpdateWeaponOrientation()
    {
        // En yak�n d��man� bul
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        if (enemies.Length > 0)
        {
            // En yak�n d��man� bul
            FindClosestEnemy();

            if (closestEnemy != null)
            {
                // Silah�n u� k�sm�n� en yak�n d��mana do�ru d�nd�r
                Vector3 directionToEnemy = closestEnemy.position - weaponTip.position;
                Quaternion rotation = Quaternion.LookRotation(directionToEnemy);
                weaponTip.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0); // Y ekseninde d�nd�r
            }
        }
    }

    // En yak�n d��man� bulma fonksiyonu
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
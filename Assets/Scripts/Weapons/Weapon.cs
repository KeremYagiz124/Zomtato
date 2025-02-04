using UnityEngine;

public enum WeaponType
{
    Shovel,
    Hoe,
    Scythe,
    Shotgun,
    AssaultRifle,
    PumpActionShotgun
}

public abstract class Weapon : MonoBehaviour
{
    public WeaponType2 weaponType;
    public float damage;  // Silah�n hasar�
    public float fireRate;  // Ate� h�z�
    public float range;  // Menzil
    public float price;  // Fiyat
    public Sprite weaponSprite;  // Silah�n g�rseli (Sprite)
    public Transform weaponTip;  // Silah�n u� noktas�n� temsil eden Transform
    public LayerMask enemyLayer;  // D��manlar�n bulundu�u layer

    // Silah�n ate� etme fonksiyonu
    public abstract void Fire(Vector3 targetPosition);

    // Silah y�kseltme fonksiyonu
    public virtual void Upgrade()
    {
        damage *= 1.2f;  // Hasar art�rma
        fireRate *= 0.9f;  // Ate� h�z�n� art�rma
        range *= 1.1f;  // Menzili art�rma
        Debug.Log($"Weapon upgraded: Damage={damage}, FireRate={fireRate}, Range={range}");
    }

    // Silah�n u� noktas�n� en yak�n d��mana do�ru d�nd�rme
    public void UpdateWeaponOrientation()
    {
        // En yak�n d��man� bul
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        if (enemies.Length > 0)
        {
            // En yak�n d��man� bul
            Transform nearestEnemy = FindNearestEnemy(enemies);

            if (nearestEnemy != null)
            {
                // Silah�n u� k�sm�n� en yak�n d��mana do�ru d�nd�r
                Vector3 directionToEnemy = nearestEnemy.position - weaponTip.position;
                Quaternion rotation = Quaternion.LookRotation(directionToEnemy);
                weaponTip.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0); // Y ekseninde d�nd�r
            }
        }
    }

    // En yak�n d��man� bulma fonksiyonu
    Transform FindNearestEnemy(Collider[] enemies)
    {
        Transform nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(weaponTip.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }
}

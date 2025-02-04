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
    public float damage;  // Silahýn hasarý
    public float fireRate;  // Ateþ hýzý
    public float range;  // Menzil
    public float price;  // Fiyat
    public Sprite weaponSprite;  // Silahýn görseli (Sprite)
    public Transform weaponTip;  // Silahýn uç noktasýný temsil eden Transform
    public LayerMask enemyLayer;  // Düþmanlarýn bulunduðu layer

    // Silahýn ateþ etme fonksiyonu
    public abstract void Fire(Vector3 targetPosition);

    // Silah yükseltme fonksiyonu
    public virtual void Upgrade()
    {
        damage *= 1.2f;  // Hasar artýrma
        fireRate *= 0.9f;  // Ateþ hýzýný artýrma
        range *= 1.1f;  // Menzili artýrma
        Debug.Log($"Weapon upgraded: Damage={damage}, FireRate={fireRate}, Range={range}");
    }

    // Silahýn uç noktasýný en yakýn düþmana doðru döndürme
    public void UpdateWeaponOrientation()
    {
        // En yakýn düþmaný bul
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        if (enemies.Length > 0)
        {
            // En yakýn düþmaný bul
            Transform nearestEnemy = FindNearestEnemy(enemies);

            if (nearestEnemy != null)
            {
                // Silahýn uç kýsmýný en yakýn düþmana doðru döndür
                Vector3 directionToEnemy = nearestEnemy.position - weaponTip.position;
                Quaternion rotation = Quaternion.LookRotation(directionToEnemy);
                weaponTip.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0); // Y ekseninde döndür
            }
        }
    }

    // En yakýn düþmaný bulma fonksiyonu
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

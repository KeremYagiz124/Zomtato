using System.Collections;
using UnityEngine;

public class FirearmWeapon : Weapon
{
    public GameObject bulletPrefab;  // Mermi prefab'ý
    public float bulletSpeed = 10f;  // Merminin hýzý
    public Transform firePoint;      // Ateþ etme noktasý

    public override void Fire(Vector3 targetPosition)
    {
        // Mermi oluþturulmasý iþlemi
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Hedefin yönünü hesapla ve mermiyi ateþle
        Vector2 direction = (targetPosition - firePoint.position).normalized;
        bulletRb.velocity = direction * bulletSpeed;

        // Ateþ etme animasyonunu tetikle
        // Eðer animasyonun tetiklenmesi gerekiyorsa burada animasyon kodu çaðrýlabilir.
        // Örneðin:
        // animator.SetTrigger("Fire");
    }

    public override void Upgrade()
    {
        // Firearm silahý için yükseltme mantýðý
        base.Upgrade();
        bulletSpeed += 2f; // Mermi hýzýný arttýr
    }
}

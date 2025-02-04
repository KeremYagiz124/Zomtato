using System.Collections;
using UnityEngine;

public class FirearmWeapon : Weapon
{
    public GameObject bulletPrefab;  // Mermi prefab'�
    public float bulletSpeed = 10f;  // Merminin h�z�
    public Transform firePoint;      // Ate� etme noktas�

    public override void Fire(Vector3 targetPosition)
    {
        // Mermi olu�turulmas� i�lemi
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Hedefin y�n�n� hesapla ve mermiyi ate�le
        Vector2 direction = (targetPosition - firePoint.position).normalized;
        bulletRb.velocity = direction * bulletSpeed;

        // Ate� etme animasyonunu tetikle
        // E�er animasyonun tetiklenmesi gerekiyorsa burada animasyon kodu �a�r�labilir.
        // �rne�in:
        // animator.SetTrigger("Fire");
    }

    public override void Upgrade()
    {
        // Firearm silah� i�in y�kseltme mant���
        base.Upgrade();
        bulletSpeed += 2f; // Mermi h�z�n� artt�r
    }
}

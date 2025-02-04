using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public GameObject weaponPrefab;  // Silah prefab'ý (Melee)
    public float attackSpeed = 10f;  // Silahýn hedefe gitme hýzý
    public float attackDistance = 2f;  // Saldýrý mesafesi

    public override void Fire(Vector3 targetPosition)
    {
        // Eðer silah Melee türünde ise ateþleme fonksiyonu farklý çalýþacak
        StartCoroutine(MeleeAttack(targetPosition));
    }

    private IEnumerator MeleeAttack(Vector3 targetPosition)
    {
        // Silah prefab'ýný baþlat
        GameObject attackInstance = Instantiate(weaponPrefab, transform.position, Quaternion.identity);

        // Hedef pozisyonu belirle
        Vector2 targetPos = targetPosition;

        // Hedefe doðru gitme hareketi
        float journeyLength = Vector2.Distance(attackInstance.transform.position, targetPos);
        float startTime = Time.time;

        // Silah hedefe doðru hareket ediyor
        while (Vector2.Distance(attackInstance.transform.position, targetPos) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * attackSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            attackInstance.transform.position = Vector2.Lerp(attackInstance.transform.position, targetPos, fractionOfJourney);
            yield return null;
        }

        // Hedefe ulaþtýktan sonra geri dönme hareketi
        startTime = Time.time;
        while (Vector2.Distance(attackInstance.transform.position, transform.position) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * attackSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            attackInstance.transform.position = Vector2.Lerp(attackInstance.transform.position, transform.position, fractionOfJourney);
            yield return null;
        }

        // Silah prefab'ýný yok et
        Destroy(attackInstance);
    }

    public override void Upgrade()
    {
        // Melee silahý için yükseltme mantýðý
        base.Upgrade();
        attackSpeed += 0.2f; // Saldýrý hýzýný arttýr
    }
}

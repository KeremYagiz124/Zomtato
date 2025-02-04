using System.Collections;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    public GameObject weaponPrefab;  // Silah prefab'� (Melee)
    public float attackSpeed = 10f;  // Silah�n hedefe gitme h�z�
    public float attackDistance = 2f;  // Sald�r� mesafesi

    public override void Fire(Vector3 targetPosition)
    {
        // E�er silah Melee t�r�nde ise ate�leme fonksiyonu farkl� �al��acak
        StartCoroutine(MeleeAttack(targetPosition));
    }

    private IEnumerator MeleeAttack(Vector3 targetPosition)
    {
        // Silah prefab'�n� ba�lat
        GameObject attackInstance = Instantiate(weaponPrefab, transform.position, Quaternion.identity);

        // Hedef pozisyonu belirle
        Vector2 targetPos = targetPosition;

        // Hedefe do�ru gitme hareketi
        float journeyLength = Vector2.Distance(attackInstance.transform.position, targetPos);
        float startTime = Time.time;

        // Silah hedefe do�ru hareket ediyor
        while (Vector2.Distance(attackInstance.transform.position, targetPos) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * attackSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            attackInstance.transform.position = Vector2.Lerp(attackInstance.transform.position, targetPos, fractionOfJourney);
            yield return null;
        }

        // Hedefe ula�t�ktan sonra geri d�nme hareketi
        startTime = Time.time;
        while (Vector2.Distance(attackInstance.transform.position, transform.position) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * attackSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            attackInstance.transform.position = Vector2.Lerp(attackInstance.transform.position, transform.position, fractionOfJourney);
            yield return null;
        }

        // Silah prefab'�n� yok et
        Destroy(attackInstance);
    }

    public override void Upgrade()
    {
        // Melee silah� i�in y�kseltme mant���
        base.Upgrade();
        attackSpeed += 0.2f; // Sald�r� h�z�n� artt�r
    }
}

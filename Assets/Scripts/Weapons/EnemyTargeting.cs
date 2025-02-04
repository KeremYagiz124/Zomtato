using UnityEngine;
using System.Linq;

public class EnemyTargeting : MonoBehaviour
{
    public Transform targetEnemy;  // Hedef d��man
    public Weapon weapon;  // Silah referans�
    public float detectionRadius = 15f;  // D��man alg�lama yar��ap�

    void Update()
    {
        SelectTarget();
        if (targetEnemy != null)
        {
            // Silah, d��man� hedef alacak
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            transform.up = direction;

            // D��man hedefte ve menzil i�inde ise ate� et
            if (Vector3.Distance(transform.position, targetEnemy.position) <= weapon.range)
            {
                weapon.Fire(targetEnemy.position);
            }
        }
    }

    private void SelectTarget()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Enemy"));
        if (enemiesInRange.Length > 0)
        {
            targetEnemy = enemiesInRange
                .Select(collider => collider.transform)
                .OrderBy(enemy => Vector3.Distance(transform.position, enemy.position)) // En yak�n d��man
                .ThenBy(enemy => enemy.GetComponent<Enemy>().health) // En az can� olan
                .FirstOrDefault();
        }
        else
        {
            targetEnemy = null;
        }
    }
}

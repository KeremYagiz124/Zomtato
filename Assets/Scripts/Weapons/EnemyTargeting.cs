using UnityEngine;
using System.Linq;

public class EnemyTargeting : MonoBehaviour
{
    public Transform targetEnemy;  // Hedef düþman
    public Weapon weapon;  // Silah referansý
    public float detectionRadius = 15f;  // Düþman algýlama yarýçapý

    void Update()
    {
        SelectTarget();
        if (targetEnemy != null)
        {
            // Silah, düþmaný hedef alacak
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            transform.up = direction;

            // Düþman hedefte ve menzil içinde ise ateþ et
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
                .OrderBy(enemy => Vector3.Distance(transform.position, enemy.position)) // En yakýn düþman
                .ThenBy(enemy => enemy.GetComponent<Enemy>().health) // En az caný olan
                .FirstOrDefault();
        }
        else
        {
            targetEnemy = null;
        }
    }
}

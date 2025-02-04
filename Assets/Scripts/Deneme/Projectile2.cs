using UnityEngine;

public class Projectile2 : MonoBehaviour
{
    [SerializeField] float speed = 12f;
    float damage;
    private void Start()
    {
        if (name == "Bullet 3(Clone)")
            damage = GameObject.Find("Weapon 3(Clone)").GetComponent<RangedWeapon2>().GetDamage();
        if (name == "Bullet 4(Clone)")
            damage = GameObject.Find("Weapon 4(Clone)").GetComponent<RangedWeapon2>().GetDamage();
        if (name == "Bullet 5(Clone)")
            damage = GameObject.Find("Weapon 5(Clone)").GetComponent<RangedWeapon2>().GetDamage();
        Destroy(gameObject, 5f);
    }
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    var enemy = collision.gameObject.GetComponent<Enemy>();
    //    if (enemy != null)
    //    {
    //        Destroy(gameObject);
    //        enemy.Hit(damage);
    //    }
    //}
    public float getDamage()
    {
        return damage;
    }
}

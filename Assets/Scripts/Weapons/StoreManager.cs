using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StoreManager : MonoBehaviour
{
    EnemySpawner es;
    PlayerController pc;
    [SerializeField] GameObject[] enemies;
    WeaponUI wuis;
    // Start is called before the first frame update
    void Start()
    {
        wuis = GameObject.Find("Weapons").GetComponent<WeaponUI>();
        es = GameObject.Find("Spawner").GetComponent<EnemySpawner>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void CloseShop()
    {
        es.CloseStore();
    }
    public void UpgradeHealth()
    {
        pc.maxHealth += 10;
        pc.setCurrentHealth(pc.maxHealth);
        es.CloseStore();
    }
    public void UpgradeMovementSpeed()
    {
        pc.moveSpeed += 1;
        es.CloseStore();
    }
    public void UpgradeCoinDropRate()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().dropRate += 0.1f;
        }
        es.CloseStore();
    }
    public void UpgradeWeapon(int weaponIndex)
    {
        //if (weaponIndex >= 0 && weaponIndex < wuis.weapons.Length)
        //{
        //    Weapon2 weapon = wuis.weapons[weaponIndex].GetComponent<Weapon2>();
        //    if (weapon != null)
        //    {
        //        weapon.Upgrade();
        //    }
        //}
        switch (weaponIndex)
        {
            case 0:
                GameObject.Find("Weapon 0(Clone)").GetComponent<MeleeWeapon2>().Upgrade();
                es.CloseStore();
                break;
            case 1:
                GameObject.Find("Weapon 1(Clone)").GetComponent<MeleeWeapon2>().Upgrade();
                es.CloseStore();
                break;
            case 2:
                GameObject.Find("Weapon 2(Clone)").GetComponent<MeleeWeapon2>().Upgrade();
                es.CloseStore();
                break;
            case 3:
                GameObject.Find("Weapon 3(Clone)").GetComponent<RangedWeapon2>().Upgrade();
                es.CloseStore();
                break;
            case 4:
                GameObject.Find("Weapon 4(Clone)").GetComponent<RangedWeapon2>().Upgrade();
                es.CloseStore();
                break;
            case 5:
                GameObject.Find("Weapon 5(Clone)").GetComponent<RangedWeapon2>().Upgrade();
                es.CloseStore();
                break;
        }
    }


    //public void Upgrade(int a)
    //{
    //    switch (a)
    //    {
    //        case 3:
    //            if (wuis.weapons[1] != null)
    //                wuis.weapons[1].GetComponent<Weapon2>().Upgrade();
    //            break;
    //        case 4:
    //            if (wuis.weapons[2] != null)
    //                wuis.weapons[2].GetComponent<Weapon2>().Upgrade();
    //            break;
    //        case 5:
    //            if (wuis.weapons[3] != null)
    //                wuis.weapons[3].GetComponent<Weapon2>().Upgrade();
    //            break;
    //        case 6:
    //            if (wuis.weapons[4] != null)
    //                wuis.weapons[4].GetComponent<Weapon2>().Upgrade();
    //            break;
    //    }
    //}
}

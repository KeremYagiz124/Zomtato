using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrades : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject weapon;
    void Start()
    {   
        
    }
    public RangedWeapon2 Weapon
    {
        get { return weapon.GetComponent<RangedWeapon2>(); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

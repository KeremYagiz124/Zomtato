using UnityEngine;

public class Shop : MonoBehaviour
{
    public PlayerInventory playerInventory;  // Oyuncunun envanteri
    public GameObject[] availableWeapons;   // Ma�azada bulunan silahlar

    // Ma�azadaki silah� sat�n alma
    //public void BuyWeapon(Weapon weapon)
    //{
    //    if (playerInventory.playerMoney >= weapon.price)
    //    {
    //        playerInventory.playerMoney -= weapon.price;
    //        playerInventory.AddWeapon(weapon);  // Silah� envantere ekle
    //        Debug.Log($"Bought {weapon.weaponType} for {weapon.price} money.");
    //    }
    //    else
    //    {
    //        Debug.Log("Not enough money to buy this weapon.");
    //    }
    //}

    // Silah y�kseltme
    //public void UpgradeWeapon(int weaponIndex)
    //{
    //    Weapon weapon = playerInventory.weapons[weaponIndex];
    //    if (weapon != null)
    //    {
    //        float upgradeCost = weapon.price * 1.5f; // Y�kseltme maliyeti
    //        if (playerInventory.playerMoney >= upgradeCost)
    //        {
    //            playerInventory.playerMoney -= upgradeCost;
    //            weapon.Upgrade();
    //            Debug.Log($"{weapon.weaponType} upgraded!");
    //        }
    //        else
    //        {
    //            Debug.Log("Not enough money to upgrade this weapon.");
    //        }
    //    }
    //}
}

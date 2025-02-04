using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public WeaponUI wui;
    //public GameObject[] weaponSlots;  // Silahlar�n yerle�tirilece�i UI slotlar�
    public GameObject canvas;  // Canvas objesi (UI'y� i�eriyor)
    public float playerMoney = 1000f;  // Oyuncunun mevcut paras�
    [SerializeField] PlayerInventory pi;

    private bool isWeaponAdded = false;  // Silah�n eklenip eklenmedi�ini kontrol et
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = transform;  // Player objesinin transform'unu al
        wui.UpdateWeaponSlots(pi);
    }
    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void Starting()
    {
        // Envanterdeki t�m silahlar� UI'ye ekle

    }

    //public void AddWeapon(Weapon2 newWeapon)
    //{
    //    // Envanterde bo� alan varsa, silah ekle
    //    for (int i = 0; i < weapons.Length; i++)
    //    {
    //        if (weapons[i] == null)
    //        {
    //            weapons[i] = newWeapon;
    //            UpdateWeaponSlot(i, newWeapon); // Silah UI'de g�sterilsin
    //            return;
    //        }
    //    }

    //    // E�er envanter doluysa, oyuncu silahlar� satmay� se�ebilir (bu k�sm� daha sonra ekleyece�iz)
    //    Debug.Log("Inventory full! You need to sell a weapon first.");
    //}

    // Silah slotlar�n� g�ncelleme fonksiyonu
    //public void UpdateWeaponSlot(int index, Weapon2 weapon)
    //{
    //    var image = weaponSlots[index].GetComponentInChildren<UnityEngine.UI.Image>();

    //    // UI'deki d�n���n s�f�rlanmas�
    //    weaponSlots[index].transform.rotation = Quaternion.identity; // D�n��� s�f�rla

    //    // UI'deki scale de�erinin s�f�rlanmas� (scale -1 veya 1 olmas�ndan etkilenmemesi i�in)
    //    weaponSlots[index].transform.localScale = new Vector3(1f, 1f, 1f);  // Sabit Scale

    //    if (weapon != null)
    //    {
    //        // Silah varsa, opakl�k 100 yap ve sprite'� g�ncelle
    //        image.sprite = weapon.weaponSprite;  // Prefab'dan sprite'� al
    //        image.color = new Color(1f, 1f, 1f, 1f);  // Opakl�k %100
    //    }
    //    else
    //    {
    //        // E�er silah yoksa, opakl�k 0 yap ve sprite'� bo� tut
    //        image.sprite = null;
    //        image.color = new Color(1f, 1f, 1f, 0f);  // Opakl�k 0
    //    }
    //}

    // Silah satma fonksiyonu
    //public void SellWeapon(int index)
    //{
    //    if (weapons[index] != null)
    //    {
    //        playerMoney += weapons[index].price;  // Sat�lan silah�n fiyat�n� ekle
    //        weapons[index] = null;  // Silah� envanterden kald�r
    //        UpdateWeaponSlot(index, null);  // UI'yi g�ncelle
    //    }
    //}

    // Canvas'� her frame'de player objesinin pozisyonuna yerle�tir
    //private void LateUpdate()
    //{
    //    // Canvas'� Player objesinin pozisyonuna yerle�tir
    //    Vector3 canvasPosition = playerTransform.position;
    //    canvas.transform.position = new Vector3(canvasPosition.x, canvasPosition.y, canvas.transform.position.z);  // Z ekseninde sabit b�rak
    //}
}

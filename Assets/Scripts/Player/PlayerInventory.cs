using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public WeaponUI wui;
    //public GameObject[] weaponSlots;  // Silahlarýn yerleþtirileceði UI slotlarý
    public GameObject canvas;  // Canvas objesi (UI'yý içeriyor)
    public float playerMoney = 1000f;  // Oyuncunun mevcut parasý
    [SerializeField] PlayerInventory pi;

    private bool isWeaponAdded = false;  // Silahýn eklenip eklenmediðini kontrol et
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
        // Envanterdeki tüm silahlarý UI'ye ekle

    }

    //public void AddWeapon(Weapon2 newWeapon)
    //{
    //    // Envanterde boþ alan varsa, silah ekle
    //    for (int i = 0; i < weapons.Length; i++)
    //    {
    //        if (weapons[i] == null)
    //        {
    //            weapons[i] = newWeapon;
    //            UpdateWeaponSlot(i, newWeapon); // Silah UI'de gösterilsin
    //            return;
    //        }
    //    }

    //    // Eðer envanter doluysa, oyuncu silahlarý satmayý seçebilir (bu kýsmý daha sonra ekleyeceðiz)
    //    Debug.Log("Inventory full! You need to sell a weapon first.");
    //}

    // Silah slotlarýný güncelleme fonksiyonu
    //public void UpdateWeaponSlot(int index, Weapon2 weapon)
    //{
    //    var image = weaponSlots[index].GetComponentInChildren<UnityEngine.UI.Image>();

    //    // UI'deki dönüþün sýfýrlanmasý
    //    weaponSlots[index].transform.rotation = Quaternion.identity; // Dönüþü sýfýrla

    //    // UI'deki scale deðerinin sýfýrlanmasý (scale -1 veya 1 olmasýndan etkilenmemesi için)
    //    weaponSlots[index].transform.localScale = new Vector3(1f, 1f, 1f);  // Sabit Scale

    //    if (weapon != null)
    //    {
    //        // Silah varsa, opaklýk 100 yap ve sprite'ý güncelle
    //        image.sprite = weapon.weaponSprite;  // Prefab'dan sprite'ý al
    //        image.color = new Color(1f, 1f, 1f, 1f);  // Opaklýk %100
    //    }
    //    else
    //    {
    //        // Eðer silah yoksa, opaklýk 0 yap ve sprite'ý boþ tut
    //        image.sprite = null;
    //        image.color = new Color(1f, 1f, 1f, 0f);  // Opaklýk 0
    //    }
    //}

    // Silah satma fonksiyonu
    //public void SellWeapon(int index)
    //{
    //    if (weapons[index] != null)
    //    {
    //        playerMoney += weapons[index].price;  // Satýlan silahýn fiyatýný ekle
    //        weapons[index] = null;  // Silahý envanterden kaldýr
    //        UpdateWeaponSlot(index, null);  // UI'yi güncelle
    //    }
    //}

    // Canvas'ý her frame'de player objesinin pozisyonuna yerleþtir
    //private void LateUpdate()
    //{
    //    // Canvas'ý Player objesinin pozisyonuna yerleþtir
    //    Vector3 canvasPosition = playerTransform.position;
    //    canvas.transform.position = new Vector3(canvasPosition.x, canvasPosition.y, canvas.transform.position.z);  // Z ekseninde sabit býrak
    //}
}

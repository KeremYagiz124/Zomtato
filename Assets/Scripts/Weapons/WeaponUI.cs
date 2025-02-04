using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public GameObject[] weapons = new GameObject[4];  // Silahlarýn yerleþtirileceði UI slotlarý
    [SerializeField] Transform playerTransform;
    public Vector3 v0, v1, v2, v3;
    private void Start()
    {
        transform.position = playerTransform.position;
        v0 = new Vector3(0.65f, 0.65f, 0);
        v1 = new Vector3(-0.65f, -0.65f, 0);
        v2 = new Vector3(0.65f, -0.65f, 0);
        v3 = new Vector3(-0.65f, 0.65f, 0);
    }
    public GameObject[] Weapons
    {
        get { return weapons; }
    }
    private void Update()
    {
        FollowPlayer();
    }
    public void UpdateWeaponSlots(PlayerInventory playerInventory)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                GameObject weaponInstance = Instantiate(weapons[i], Vector3.zero, Quaternion.identity);
                weaponInstance.transform.SetParent(transform);
                weaponInstance.transform.position = new Vector3(0.65f * (i % 2 == 0 ? 1 : -1), 0.65f * (i < 2 ? 1 : -1), 0);
                weaponInstance.GetComponent<RangedWeapon2>().WeaponIndex = weapons[i].GetComponent<RangedWeapon2>().WeaponIndex;
                weapons[i] = weaponInstance; // Silahý güncelle
            }
        }
    }
    void FollowPlayer()
    {
        playerTransform = GameObject.Find("Player").transform;
        transform.position = playerTransform.position;
    }
}

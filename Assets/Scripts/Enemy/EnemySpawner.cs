using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Düþman prefab'larý
    public GameObject spawnMarkerPrefab;  // Doðacak düþman için iþaret prefab'ý
    public float spawnRadius = 5f;  // Düþmanýn doðacaðý alanýn yarýçapý
    public float initialSpawnTime = 3f;  // Baþlangýçta düþman doðma süresi
    public float spawnTimeDecreaseRate = 0.1f;  // Zorluk arttýkça doðma süresi kýsalýr
    public float spawnChanceIncreaseRate = 0.05f;  // Zorluk arttýkça doðma oraný artacak

    public int currentLevel = 1;  // Mevcut bölüm
    private float spawnTime;  // Düþman doðma süresi
    private float spawnChance;  // Düþman doðma oraný
    private Transform player;  // Oyuncu objesinin referansý

    private bool isSpawning = true; // Düþman doðurma kontrolü
    [SerializeField] float levelTransitionTime = 5f;  // Bölüm geçiþ süresi (20 saniye)
    private float levelTransitionTimer;  // Bölüm geçiþi için sayaç

    public GameObject storeUI;  // Maðaza UI'si referansý
    public bool isStoreActive = false; // Maðaza açýk mý?

    private PlayerController playerController;  // Karakterin hareket kontrolünü referans alacaðýz
    public GameObject wui;
    WeaponUI wuis;
    int sayac = 0;
    public int u1, u2, u3;

    WeaponUpgrades wu;

    [SerializeField] GameObject[] upgrades;


    PlayerController playerObject;


    void Start()
    {
        wu = null;
        wuis = wui.GetComponent<WeaponUI>();
        spawnTime = initialSpawnTime;  // Baþlangýçta belirlediðimiz zaman
        spawnChance = spawnChanceIncreaseRate * currentLevel;  // Baþlangýçta oran
        player = GameObject.FindGameObjectWithTag("Player").transform; // Oyuncuyu bul
        playerController = player.GetComponent<PlayerController>(); // Karakterin hareket kontrolünü al
        levelTransitionTimer = levelTransitionTime;  // Sayaç baþlangýcý
        foreach (var a in upgrades)
        {
            a.SetActive(false);
        }
        storeUI.SetActive(false);  // Maðaza baþlangýçta kapalý
        StartCoroutine(SpawnEnemies());
        playerObject = GameObject.Find("Player").GetComponent<PlayerController>();
        for (int i = 0; i < upgrades.Length; i++)
        {
            wu = upgrades[i].GetComponent<WeaponUpgrades>();
            if (wu != null)
            {
                wu.Weapon.WeaponIndex = i;
            }
        }
    }

    void Update()
    {
        // Escape tuþuna basýldýðýnda maðazayý kapatma iþlemi
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseStore();
        }

        // Zorluk arttýkça spawnTime ve spawnChance artacak
        if (!isSpawning) return;

        spawnTime = Mathf.Max(1f, initialSpawnTime - currentLevel * spawnTimeDecreaseRate);
        spawnChance = Mathf.Min(1f, spawnChanceIncreaseRate * currentLevel);

        // 20 saniyelik sayaç iþlemi
        levelTransitionTimer -= Time.deltaTime;
        if (levelTransitionTimer <= 0)
        {
            // Yeni bölüme geçiþ yap
            TransitionToNextLevel();
            levelTransitionTimer = levelTransitionTime;  // Sayaç sýfýrlanýr
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnTime);  // SpawnTime kadar bekle

            // Düþman doðma þansý
            if (UnityEngine.Random.value < spawnChance)
            {
                // Spawn öncesi iþaret oluþtur
                Vector2 spawnPosition = GetRandomSpawnPosition();
                GameObject spawnMarker = Instantiate(spawnMarkerPrefab, spawnPosition, Quaternion.identity);

                // 0.5 saniye iþaretin görünmesini bekle
                yield return new WaitForSeconds(0.5f);

                // Ýþaretin oluþturulmasý tamamlandýktan sonra düþmaný doður
                Destroy(spawnMarker, 0.5f);  // Ýþareti yok et

                // Seviye arttýkça doðma ihtimali artan düþmanlarý kontrol et
                if (currentLevel >= 1 && currentLevel < 7)
                {
                    // Ýlk 2 düþman (1. ve 2. düþman) doðma ihtimali var
                    TrySpawnEnemy(0, spawnPosition);  // 1. düþman
                    TrySpawnEnemy(1, spawnPosition);  // 2. düþman
                }

                if (currentLevel >= 7 && currentLevel < 15)
                {
                    // 3. ve 4. düþmanlarýn doðma ihtimali var
                    TrySpawnEnemy(2, spawnPosition);  // 3. düþman
                    TrySpawnEnemy(3, spawnPosition);  // 4. düþman
                }

                if (currentLevel >= 15)
                {
                    // 5. düþman doðma ihtimali var
                    TrySpawnEnemy(4, spawnPosition);  // 5. düþman
                }
            }
        }
    }

    // Rastgele bir doðma pozisyonu hesaplamak
    Vector2 GetRandomSpawnPosition()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return Vector2.zero;
        }

        // Oyuncunun pozisyonunu al
        Vector2 playerPosition = player.position;

        // Oyuncunun etrafýnda rastgele bir pozisyon oluþtur
        float randomX = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        float randomY = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        Vector2 randomPos = new Vector2(randomX, randomY);

        // Oyuncu etrafýndaki pozisyonu hesapla
        Vector2 spawnPosition = playerPosition + randomPos;
        return spawnPosition;
    }

    // Düþmaný doðurma fonksiyonu
    void TrySpawnEnemy(int enemyIndex, Vector2 spawnPosition)
    {
        if (enemyPrefabs.Length > enemyIndex)
        {
            // Düþman prefab'ýný seç ve doður
            Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
        }
    }

    // Bölüm geçiþi fonksiyonu
    void TransitionToNextLevel()
    {
        currentLevel++;  // Bir sonraki bölüme geçiþ
        Debug.Log("Level " + currentLevel + " reached!");

        // Ýlgili ayarlamalarý yap (örneðin, spawn zamanýný kýsaltmak, spawn oranýný arttýrmak)
        spawnTime = Mathf.Max(1f, initialSpawnTime - currentLevel * spawnTimeDecreaseRate);
        spawnChance = Mathf.Min(1f, spawnChanceIncreaseRate * currentLevel);

        // Önceki seviyedeki tüm düþmanlarý öldür
        DestroyAllEnemies();

        // Maðazayý aç
        OpenStore();
    }

    // Maðazayý açan fonksiyon
    void OpenStore()
    {
        if (!isStoreActive)
        {
            isStoreActive = true;
            storeUI.SetActive(true);  // Maðazayý aktif et
            StopSpawning();  // Maðaza açýldýðýnda düþman doðumunu durdur
            playerController.DisableMovement();  // Karakterin hareketini engelle
            Debug.Log("Store is now open!");
            playerObject.unDead = true;

            GameObject[] ownedWeapons = wuis.Weapons;
            int count = 3;
            HashSet<int> set = new HashSet<int>();
            for (int i = 0; i < ownedWeapons.Length; i++)
            {
                set.Add(ownedWeapons[i].GetComponent<RangedWeapon2>().WeaponIndex);
            }
            count += set.Count;
            List<int> setList = new List<int>(set);


            u1 = UnityEngine.Random.Range(0, count);
            u2 = UnityEngine.Random.Range(0, count);
            u3 = UnityEngine.Random.Range(0, count);
            while (u1 == u2 || u1 == u3 || u2 == u3)
            {
                u1 = UnityEngine.Random.Range(0, count);
                u2 = UnityEngine.Random.Range(0, count);
                u3 = UnityEngine.Random.Range(0, count);
            }

            Upgrades(u1, setList);
            Upgrades(u2, setList);
            Upgrades(u3, setList);
            float maxHealth = playerController.getMaxHealth();
            playerController.setCurrentHealth(maxHealth);
        }
    }

    // Maðazayý kapatan fonksiyon
    public void CloseStore()
    {
        if (isStoreActive)
        {
            isStoreActive = false;
            storeUI.SetActive(false);  // Maðazayý kapat
            ResumeSpawning();  // Maðaza kapandýktan sonra düþman doðumunu baþlat
            playerController.EnableMovement();  // Karakterin hareketini tekrar etkinleþtir
            Debug.Log("Store is now closed!");
            playerObject.unDead = false;
            sayac = 0;

            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].SetActive(false);
            }
        }
    }

    // Düþmanlarý yok etme fonksiyonu
    void DestroyAllEnemies()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            Destroy(enemy.gameObject);
        }
    }

    // Düþman doðumunu durdurmak için fonksiyon
    public void StopSpawning()
    {
        isSpawning = false;
    }

    // Düþman doðumunu baþlatmak için fonksiyon
    public void ResumeSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnEnemies());
    }

    void Upgrades(int value, List<int> setList)
    {
        if (value >= 3 && value <= 8)
        {

            // Silah yükseltmesi yapýlacaksa, mevcut silahlarý kontrol et ve ona göre yükseltme yap
            int selectedWeapon = UnityEngine.Random.Range(0, setList.Count); // Silahý seç
            int x = setList[selectedWeapon];
            upgrades[x].SetActive(true); // Silahýn yükseltme butonunu göster
            sayac++;
            upgrades[x].transform.position = Positioning(sayac); // Konumlandýr
        }
        else
        {
            upgrades[value].SetActive(true); // Diðer yükseltmeler için butonu göster
            sayac++;
            upgrades[value].transform.position = Positioning(sayac);
        }
    }



    Vector2 Positioning(int value)
    {
        if (value == 1)
        {
            return new Vector2(109.48f, 132.025f);
        }
        else if (value == 2)
        {
            return new Vector2(297.16f, 132.025f);
        }
        else if (value == 3)
        {
            return new Vector2(484.84f, 132.025f);
        }
        return new Vector2(261f, 57.64f);
    }

}
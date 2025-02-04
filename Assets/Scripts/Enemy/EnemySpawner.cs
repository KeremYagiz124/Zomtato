using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // D��man prefab'lar�
    public GameObject spawnMarkerPrefab;  // Do�acak d��man i�in i�aret prefab'�
    public float spawnRadius = 5f;  // D��man�n do�aca�� alan�n yar��ap�
    public float initialSpawnTime = 3f;  // Ba�lang��ta d��man do�ma s�resi
    public float spawnTimeDecreaseRate = 0.1f;  // Zorluk artt�k�a do�ma s�resi k�sal�r
    public float spawnChanceIncreaseRate = 0.05f;  // Zorluk artt�k�a do�ma oran� artacak

    public int currentLevel = 1;  // Mevcut b�l�m
    private float spawnTime;  // D��man do�ma s�resi
    private float spawnChance;  // D��man do�ma oran�
    private Transform player;  // Oyuncu objesinin referans�

    private bool isSpawning = true; // D��man do�urma kontrol�
    [SerializeField] float levelTransitionTime = 5f;  // B�l�m ge�i� s�resi (20 saniye)
    private float levelTransitionTimer;  // B�l�m ge�i�i i�in saya�

    public GameObject storeUI;  // Ma�aza UI'si referans�
    public bool isStoreActive = false; // Ma�aza a��k m�?

    private PlayerController playerController;  // Karakterin hareket kontrol�n� referans alaca��z
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
        spawnTime = initialSpawnTime;  // Ba�lang��ta belirledi�imiz zaman
        spawnChance = spawnChanceIncreaseRate * currentLevel;  // Ba�lang��ta oran
        player = GameObject.FindGameObjectWithTag("Player").transform; // Oyuncuyu bul
        playerController = player.GetComponent<PlayerController>(); // Karakterin hareket kontrol�n� al
        levelTransitionTimer = levelTransitionTime;  // Saya� ba�lang�c�
        foreach (var a in upgrades)
        {
            a.SetActive(false);
        }
        storeUI.SetActive(false);  // Ma�aza ba�lang��ta kapal�
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
        // Escape tu�una bas�ld���nda ma�azay� kapatma i�lemi
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseStore();
        }

        // Zorluk artt�k�a spawnTime ve spawnChance artacak
        if (!isSpawning) return;

        spawnTime = Mathf.Max(1f, initialSpawnTime - currentLevel * spawnTimeDecreaseRate);
        spawnChance = Mathf.Min(1f, spawnChanceIncreaseRate * currentLevel);

        // 20 saniyelik saya� i�lemi
        levelTransitionTimer -= Time.deltaTime;
        if (levelTransitionTimer <= 0)
        {
            // Yeni b�l�me ge�i� yap
            TransitionToNextLevel();
            levelTransitionTimer = levelTransitionTime;  // Saya� s�f�rlan�r
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnTime);  // SpawnTime kadar bekle

            // D��man do�ma �ans�
            if (UnityEngine.Random.value < spawnChance)
            {
                // Spawn �ncesi i�aret olu�tur
                Vector2 spawnPosition = GetRandomSpawnPosition();
                GameObject spawnMarker = Instantiate(spawnMarkerPrefab, spawnPosition, Quaternion.identity);

                // 0.5 saniye i�aretin g�r�nmesini bekle
                yield return new WaitForSeconds(0.5f);

                // ��aretin olu�turulmas� tamamland�ktan sonra d��man� do�ur
                Destroy(spawnMarker, 0.5f);  // ��areti yok et

                // Seviye artt�k�a do�ma ihtimali artan d��manlar� kontrol et
                if (currentLevel >= 1 && currentLevel < 7)
                {
                    // �lk 2 d��man (1. ve 2. d��man) do�ma ihtimali var
                    TrySpawnEnemy(0, spawnPosition);  // 1. d��man
                    TrySpawnEnemy(1, spawnPosition);  // 2. d��man
                }

                if (currentLevel >= 7 && currentLevel < 15)
                {
                    // 3. ve 4. d��manlar�n do�ma ihtimali var
                    TrySpawnEnemy(2, spawnPosition);  // 3. d��man
                    TrySpawnEnemy(3, spawnPosition);  // 4. d��man
                }

                if (currentLevel >= 15)
                {
                    // 5. d��man do�ma ihtimali var
                    TrySpawnEnemy(4, spawnPosition);  // 5. d��man
                }
            }
        }
    }

    // Rastgele bir do�ma pozisyonu hesaplamak
    Vector2 GetRandomSpawnPosition()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found!");
            return Vector2.zero;
        }

        // Oyuncunun pozisyonunu al
        Vector2 playerPosition = player.position;

        // Oyuncunun etraf�nda rastgele bir pozisyon olu�tur
        float randomX = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        float randomY = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
        Vector2 randomPos = new Vector2(randomX, randomY);

        // Oyuncu etraf�ndaki pozisyonu hesapla
        Vector2 spawnPosition = playerPosition + randomPos;
        return spawnPosition;
    }

    // D��man� do�urma fonksiyonu
    void TrySpawnEnemy(int enemyIndex, Vector2 spawnPosition)
    {
        if (enemyPrefabs.Length > enemyIndex)
        {
            // D��man prefab'�n� se� ve do�ur
            Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);
        }
    }

    // B�l�m ge�i�i fonksiyonu
    void TransitionToNextLevel()
    {
        currentLevel++;  // Bir sonraki b�l�me ge�i�
        Debug.Log("Level " + currentLevel + " reached!");

        // �lgili ayarlamalar� yap (�rne�in, spawn zaman�n� k�saltmak, spawn oran�n� artt�rmak)
        spawnTime = Mathf.Max(1f, initialSpawnTime - currentLevel * spawnTimeDecreaseRate);
        spawnChance = Mathf.Min(1f, spawnChanceIncreaseRate * currentLevel);

        // �nceki seviyedeki t�m d��manlar� �ld�r
        DestroyAllEnemies();

        // Ma�azay� a�
        OpenStore();
    }

    // Ma�azay� a�an fonksiyon
    void OpenStore()
    {
        if (!isStoreActive)
        {
            isStoreActive = true;
            storeUI.SetActive(true);  // Ma�azay� aktif et
            StopSpawning();  // Ma�aza a��ld���nda d��man do�umunu durdur
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

    // Ma�azay� kapatan fonksiyon
    public void CloseStore()
    {
        if (isStoreActive)
        {
            isStoreActive = false;
            storeUI.SetActive(false);  // Ma�azay� kapat
            ResumeSpawning();  // Ma�aza kapand�ktan sonra d��man do�umunu ba�lat
            playerController.EnableMovement();  // Karakterin hareketini tekrar etkinle�tir
            Debug.Log("Store is now closed!");
            playerObject.unDead = false;
            sayac = 0;

            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].SetActive(false);
            }
        }
    }

    // D��manlar� yok etme fonksiyonu
    void DestroyAllEnemies()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            Destroy(enemy.gameObject);
        }
    }

    // D��man do�umunu durdurmak i�in fonksiyon
    public void StopSpawning()
    {
        isSpawning = false;
    }

    // D��man do�umunu ba�latmak i�in fonksiyon
    public void ResumeSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnEnemies());
    }

    void Upgrades(int value, List<int> setList)
    {
        if (value >= 3 && value <= 8)
        {

            // Silah y�kseltmesi yap�lacaksa, mevcut silahlar� kontrol et ve ona g�re y�kseltme yap
            int selectedWeapon = UnityEngine.Random.Range(0, setList.Count); // Silah� se�
            int x = setList[selectedWeapon];
            upgrades[x].SetActive(true); // Silah�n y�kseltme butonunu g�ster
            sayac++;
            upgrades[x].transform.position = Positioning(sayac); // Konumland�r
        }
        else
        {
            upgrades[value].SetActive(true); // Di�er y�kseltmeler i�in butonu g�ster
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    PlayerController pc;
    public Text healthText, coinText;
    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthUI();
        UpdateCoinUI();
    }
    void UpdateHealthUI()
    {
        // Saðlýk bilgisini al ve UI'ya yaz
        if (pc != null)
        {
            healthText.text = pc.getCurrentHealth().ToString();  // Mevcut caný göster
        }
    }

    void UpdateCoinUI()
    {
        // Para bilgisini al ve UI'ya yaz
        int currentCoins = pc.GetCoins();  // GetCoins fonksiyonu, oyuncunun parasýný döndüren bir fonksiyon
        coinText.text = currentCoins.ToString();  // Mevcut parayý göster
    }
}


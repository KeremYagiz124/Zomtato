using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject healthItemPrefab; // Can eþyasý
    public GameObject magnetItemPrefab; // Mýknatýs eþyasý
    public GameObject highValueCoinPrefab; // En yüksek seviye para

    private bool isOpened = false; // Kutu açýldý mý kontrolü

    // Karakter kutuya dokunduðunda çalýþacak fonksiyon
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Sadece Player kutuya dokunduðunda çalýþsýn
        if (collision.CompareTag("Player") && !isOpened)
        {
            // Kutu açýldýðýnda eþya düþür
            OpenBox();
            isOpened = true; // Kutu bir kez açýldý, tekrar açýlmaz
        }
    }

    // Kutu açýldýðýnda eþya düþürme
    public void OpenBox()
    {
        DropItem();

        // Kutu açýldýðýnda bir animasyon veya efekt tetikleyebilirsiniz (isteðe baðlý)
        Destroy(gameObject); // Kutuyu yok et
    }

    // Rastgele bir eþya düþürme fonksiyonu
    void DropItem()
    {
        float randomValue = Random.value;  // 0-1 arasýnda rastgele bir deðer

        // Eþya seçimi
        if (randomValue < 0.33f)  // %33 ihtimalle can
        {
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
        else if (randomValue < 0.66f)  // %33 ihtimalle mýknatýs
        {
            Instantiate(magnetItemPrefab, transform.position, Quaternion.identity);
        }
        else  // %34 ihtimalle en yüksek seviye para
        {
            Instantiate(highValueCoinPrefab, transform.position, Quaternion.identity);
        }
    }
}

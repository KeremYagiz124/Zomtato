using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject healthItemPrefab; // Can e�yas�
    public GameObject magnetItemPrefab; // M�knat�s e�yas�
    public GameObject highValueCoinPrefab; // En y�ksek seviye para

    private bool isOpened = false; // Kutu a��ld� m� kontrol�

    // Karakter kutuya dokundu�unda �al��acak fonksiyon
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Sadece Player kutuya dokundu�unda �al��s�n
        if (collision.CompareTag("Player") && !isOpened)
        {
            // Kutu a��ld���nda e�ya d���r
            OpenBox();
            isOpened = true; // Kutu bir kez a��ld�, tekrar a��lmaz
        }
    }

    // Kutu a��ld���nda e�ya d���rme
    public void OpenBox()
    {
        DropItem();

        // Kutu a��ld���nda bir animasyon veya efekt tetikleyebilirsiniz (iste�e ba�l�)
        Destroy(gameObject); // Kutuyu yok et
    }

    // Rastgele bir e�ya d���rme fonksiyonu
    void DropItem()
    {
        float randomValue = Random.value;  // 0-1 aras�nda rastgele bir de�er

        // E�ya se�imi
        if (randomValue < 0.33f)  // %33 ihtimalle can
        {
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
        else if (randomValue < 0.66f)  // %33 ihtimalle m�knat�s
        {
            Instantiate(magnetItemPrefab, transform.position, Quaternion.identity);
        }
        else  // %34 ihtimalle en y�ksek seviye para
        {
            Instantiate(highValueCoinPrefab, transform.position, Quaternion.identity);
        }
    }
}

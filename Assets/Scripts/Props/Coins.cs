using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    // Start is called before the first frame update
    int value;
    private bool isCollected = false;  // Coin'in toplanýp toplanmadýðýný kontrol et
    SpriteRenderer sr;
    [SerializeField] float fadeSpeed = 0.00000000000001f;
    GameObject coinImage;

    // Private field to store the state of 'isActive'
    private bool isActive;

    void Start()
    {
        coinImage = GameObject.FindWithTag("CoinUI");
        sr = GetComponent<SpriteRenderer>();

        if (name == "Coin0(Clone)")
            value = 5;
        else if (name == "Coin1(Clone)")
            value = 10;
        else if (name == "Coin2(Clone)")
            value = 20;
        else
            value = 0;
    }

    private void Update()
    {
        if (isActive)
            Fading();
    }

    public int GetValue()
    {
        return value;
    }

    //// Coin'in toplandýðýný iþaretleyen metod
    //public void MarkAsCollected()
    //{
    //    isCollected = true;
    //}

    //// Coin'in toplanýp toplanmadýðýný kontrol eden metod
    //public bool IsCollected()
    //{
    //    return isCollected;
    //}

    public void Fading()
    {
        // Opaklýk deðerini azalt
        Color color = sr.color;
        color.a -= fadeSpeed * 2 * Time.deltaTime; // Opaklýðý yavaþça azalt
        sr.color = color; // Yeni rengi uygula
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            collision.gameObject.GetComponent<PlayerController>().Coin += GetValue();
            Debug.Log(collision.gameObject.GetComponent<PlayerController>().Coin);
            isActive = true;
            coinImage.GetComponent<Animator>().SetTrigger("Collected");
        }
    }
}

using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    public float magnetDuration = 5f;  // Mýknatýs etkisinin süresi
    private bool isActive = false;     // Mýknatýs aktif mi?
    private PlayerController playerController;
    SpriteRenderer sr;
    [SerializeField] float fadeSpeed = 0.00000000000001f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (isActive)
            Fading();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Eðer Player mýknatýsý alýrsa
        if (collider.CompareTag("Player"))
        {
            playerController = collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                ActivateMagnet();
                isActive = true;
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 5f);  // Mýknatýs itemini yok et
            }
        }
    }

    // Mýknatýs etkinleþtirildiðinde
    void ActivateMagnet()
    {
        if (playerController != null)
        {
            playerController.StartCoroutine(playerController.ActivateMagnet(magnetDuration));
        }
    }
    public void Fading()
    {
        // Opaklýk deðerini azalt
        Color color = sr.color;
        color.a -= fadeSpeed * Time.deltaTime; // Opaklýðý yavaþça azalt
        sr.color = color; // Yeni rengi uygula

    }

}

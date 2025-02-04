using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    public float magnetDuration = 5f;  // M�knat�s etkisinin s�resi
    private bool isActive = false;     // M�knat�s aktif mi?
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
        // E�er Player m�knat�s� al�rsa
        if (collider.CompareTag("Player"))
        {
            playerController = collider.GetComponent<PlayerController>();
            if (playerController != null)
            {
                ActivateMagnet();
                isActive = true;
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 5f);  // M�knat�s itemini yok et
            }
        }
    }

    // M�knat�s etkinle�tirildi�inde
    void ActivateMagnet()
    {
        if (playerController != null)
        {
            playerController.StartCoroutine(playerController.ActivateMagnet(magnetDuration));
        }
    }
    public void Fading()
    {
        // Opakl�k de�erini azalt
        Color color = sr.color;
        color.a -= fadeSpeed * Time.deltaTime; // Opakl��� yava��a azalt
        sr.color = color; // Yeni rengi uygula

    }

}

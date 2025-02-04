using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject On;
    [SerializeField] GameObject Off;
    bool isPressed;
    public Camera mainCamera; // Ana kameran�z� buraya atay�n

    private List<Resolution> cozunurlukler = new List<Resolution>();
    private int mevcutCozunurlukIndex = 0;

    [SerializeField] GameObject MainMenu;
    [SerializeField] Text ResText;

    void Start()
    {
        isPressed = false;
        Off.SetActive(false);

        // Kullan�labilir ��z�n�rl�kleri listeleyin
        Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            cozunurlukler.Add(res);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ResText.text = cozunurlukler[mevcutCozunurlukIndex].ToString();
    }
    public void SoundButton()
    {
        //if (isPressed)
        //{
        //    On.SetActive(true);
        //    Off.SetActive(false);
        //    mainCamera.GetComponent<AudioListener>().enabled = true;
        //    isPressed = false;
        //}
        //if (!isPressed)
        //{
        //    On.SetActive(false);
        //    Off.SetActive(true);
        //    mainCamera.GetComponent<AudioListener>().enabled = false;
        //    isPressed = true;
        //}
        isPressed = !isPressed; // Durumu tersine �evir

        On.SetActive(isPressed);
        Off.SetActive(!isPressed);
    }
    public void ResolutionButton()
    {
        // Bir sonraki ��z�n�rl��e ge�
        mevcutCozunurlukIndex++;
        if (mevcutCozunurlukIndex >= cozunurlukler.Count)
        {
            mevcutCozunurlukIndex = 0; // Ba�a d�n
        }

        // ��z�n�rl��� ayarla
        Resolution selectedResolution = cozunurlukler[mevcutCozunurlukIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, true); // Tam ekran modunda

    }
    public void GoToMainMenu()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

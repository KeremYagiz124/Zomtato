using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject SettingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        SettingsMenu.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Settings()
    {
        SettingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}

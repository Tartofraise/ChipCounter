using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    public GameObject bt;

    private void Start()
    {
        if (PlayerPrefs.GetString("names") == "")
        {
            bt.gameObject.SetActive(false);
        }
        Debug.Log(PlayerPrefs.GetString("names"));
    }

    private void Update()
    {
    }

    public void jouer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("setup");
        //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("menu_V");
    }

    public void continuer()
    {
        switch (PlayerPrefs.GetString("type"))
        {
            case "p":
                UnityEngine.SceneManagement.SceneManager.LoadScene("poker");
                break;

            case "c":
                UnityEngine.SceneManagement.SceneManager.LoadScene("chip");
                break;

            default:
                break;
        }

        chip.loadsave();
    }
}
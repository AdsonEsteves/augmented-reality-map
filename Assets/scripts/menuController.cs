using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuController : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject textoParte1;
    public GameObject textoParte2;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenTrackingScene()
    {
        SceneManager.LoadScene("mapTracker");
    }

    public void BaixarImagens()
    {
        Application.OpenURL("https://cdn.discordapp.com/attachments/571157550956019741/833569291576016896/todosostiles2.png");
    }

    public void AbrirTutorial()
    {
        tutorial.SetActive(true);
    }

    public void fecharTutorial()
    {
        tutorial.SetActive(false);
    }

    public void mostrarTutoParte1()
    {
        textoParte1.SetActive(true);
        textoParte2.SetActive(false);
    }

    public void mostrarTutoParte2()
    {
        textoParte1.SetActive(false);
        textoParte2.SetActive(true);
    }
}

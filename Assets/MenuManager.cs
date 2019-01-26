using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject optionsMenu;
    public GameObject playDecal;
    public GameObject optionsDecal;
    public GameObject quitDecal;

    public GameObject gameUI;
    public GameObject menuUI;


    private void OnEnable() {
        if (Instance != null) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame() {
        playDecal.SetActive(false);
        optionsDecal.SetActive(false);
        quitDecal.SetActive(false);
        gameUI.SetActive(true);
        menuUI.SetActive(false);
        GameManager.instance.StartGame();
    }

    public void GoToOptions() {
        optionsMenu.SetActive(true);
    }

    public void CloseOptions() {
        optionsMenu.SetActive(false);
    }

    public void Quit() {
        Application.Quit();
    }
}

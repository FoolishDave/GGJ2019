using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject optionsMenu;

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

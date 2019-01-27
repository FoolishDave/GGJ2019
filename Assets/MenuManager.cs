using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject optionsMenu;
    public GameObject playDecal;
    public GameObject optionsDecal;
    public GameObject quitDecal;
    public GameObject questionDecal;
    public TextMeshProUGUI questionText;

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
        questionDecal.SetActive(false);
        UIManager.instance.HideWinText();
        GameManager.instance.StartGame();
    }

    public void ShowMenu() {
        playDecal.SetActive(true);
        optionsDecal.SetActive(true);
        quitDecal.SetActive(true);
        gameUI.SetActive(false);
        menuUI.SetActive(true);
        questionDecal.SetActive(true);
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

    public void GetHelp() {
        Debug.Log("getting help");
        questionText.DOFade(1f, 4f).OnComplete(() => questionText.DOFade(0f, .5f));
    }
}

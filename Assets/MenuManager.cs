using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject optionsMenu;
    public GameObject playDecal;
    public GameObject optionsDecal;
    public GameObject quitDecal;
    public GameObject questionDecal;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI resolutionText;
    public TextMeshProUGUI refreshRateText;
    public AudioMixer mixer;
    public GameObject gameUI;
    public GameObject menuUI;
    public Slider volumeSlider;
    public Slider sfxSlider;
    public Slider musicSlider;


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
        Debug.Log("Starting game");
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
        Resolution res = Screen.currentResolution;
        resolutionText.text = res.width + "x" + res.height;
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

    public void RaiseResolution() {
        int index = Array.IndexOf(Screen.resolutions, Screen.currentResolution) + 1;
        if (index >= Screen.resolutions.Length) index = Screen.resolutions.Length - 1;
        Screen.SetResolution(Screen.resolutions[index].width, Screen.resolutions[index].height, true);
        Resolution res = Screen.currentResolution;
        resolutionText.text = res.width + "x" + res.height;
    }

    public void LowerResolution() {
        int index = Array.IndexOf(Screen.resolutions, Screen.currentResolution) - 1;
        if (index < 0) index = 0;
        Screen.SetResolution(Screen.resolutions[index].width, Screen.resolutions[index].height, true);
        Resolution res = Screen.currentResolution;
        resolutionText.text = res.width + "x" + res.height;
    }

    public void ChangeMasterVolume() {
        mixer.SetFloat("MasterVolume", ConvertToDecibel(volumeSlider.value));
    }

    public void ChangeSfxVolume() {
        mixer.SetFloat("SFXVolume", ConvertToDecibel(sfxSlider.value));
    }

    public void ChangeMusicVolume() {
        mixer.SetFloat("MusicVolume", ConvertToDecibel(musicSlider.value));
    }

    private float ConvertToDecibel(float _value) {
        return Mathf.Log10(Mathf.Max(_value, 0.0001f)) * 20f;
    }
}

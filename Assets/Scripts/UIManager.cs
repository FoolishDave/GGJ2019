using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winText;

    public GameObject scoreTextPrefab;
    public Transform scoreTextParent;


    void Awake() {
        instance = this;
        HideWinText();
    }

    public void UpdateRoundUI(RoundAbstract r) {
        titleText.text = r.title;
        descText.text = r.desc;
    }

    public void UpdateTimerUI(float time) {
        timerText.enabled = time > 0;
        timerText.text = ((int) time).ToString();
    }

    public void CreateScoreUI() {
        /*
        int index = 0;
        for (int id = 0; id < PlayerManager.Instance.Players.Length; id++) {
            GameObject t = Instantiate(scoreTextPrefab);
            t.transform.SetParent(scoreTextParent);
            RectTransform textRT = t.GetComponent<RectTransform>();
            RectTransform prefabRT = scoreTextPrefab.GetComponent<RectTransform>();
            textRT.anchorMin = prefabRT.anchorMin;
            textRT.anchorMax = prefabRT.anchorMax;
            textRT.anchoredPosition = prefabRT.anchoredPosition;
            textRT.localScale = prefabRT.localScale;

            Vector2 pos = textRT.anchoredPosition;
            pos.y += (index * textRT.rect.height);
            textRT.anchoredPosition = pos;

            index++;
        }
        scoreTextPrefab.SetActive(false);*/
        UpdateScoreUI();
    }

    public void UpdateScoreUI() {
        scoreText.text = "";
        for(int i = 0; i < PlayerManager.Instance.NumPlayers; i++) {
            if (PlayerManager.Instance.Players[i] != null) {
                scoreText.text += "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerManager.Instance.playerColors[i]) + ">P" + (i + 1) + ":</color> " + GameManager.playerScores[i] + "\n";
            }
        }
    }

    public void ShowWinText(List<int> winner) {
        string winStr = "";
        foreach(int i in winner) {
            winStr +=  "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerManager.Instance.playerColors[i]) + "><b>P" + (i + 1) + "</b></color> ";
        }
        winStr += " Wins!";
        winText.text = winStr;
        winText.enabled = true;
    }

    public void HideWinText() {
        winText.enabled = false;
    }
}

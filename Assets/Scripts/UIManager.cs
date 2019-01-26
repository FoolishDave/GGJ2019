using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;

    public TextMeshProUGUI timerText;

    public List<TextMeshProUGUI> scoreTexts = new List<TextMeshProUGUI>();
    public GameObject scoreTextPrefab;
    public Transform scoreTextParent;


    void Awake() {
        instance = this;
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

            scoreTexts.Add(t.GetComponent<TextMeshProUGUI>());
            index++;
        }
        scoreTextPrefab.SetActive(false);
        UpdateScoreUI();
    }

    public void UpdateScoreUI() {
        for(int i = 0; i < scoreTexts.Count; i++) {
            scoreTexts[i].text = "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerManager.Instance.playerColors[i]) + ">Player " + i + ":</color> " + GameManager.playerScores[i];
        }
    }
}

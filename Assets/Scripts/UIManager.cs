using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;

    public TextMeshProUGUI timerText;

    public Dictionary<int, TextMeshProUGUI> scoreTexts = new Dictionary<int, TextMeshProUGUI>();
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
        timerText.text = ((int) time).ToString();
    }

    public void CreateScoreUI() {
        int index = 0;
        foreach (int id in GameManager.playerIDs) {
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

            scoreTexts.Add(id, t.GetComponent<TextMeshProUGUI>());
            index++;
        }
        scoreTextPrefab.SetActive(false);
        UpdateScoreUI();
    }

    public void UpdateScoreUI() {
        foreach(KeyValuePair<int, TextMeshProUGUI> p in scoreTexts) {
            p.Value.text = "Player " + p.Key + ": " + GameManager.playerScores[p.Key];
        }
    }
}

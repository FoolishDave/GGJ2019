using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;

    public TextMeshProUGUI timerText;

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
}

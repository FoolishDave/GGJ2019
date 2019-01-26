using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    public static RoundManager instance;
    public List<RoundAbstract> rounds;
    private RoundAbstract currentRound;

    public delegate void Callback();
    public Callback timerCallback;

    private float timer;

    void Awake() {
        instance = this;
        NextRound();
    }

    void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
            UIManager.instance.UpdateTimerUI(timer);
            if (timer < 0) {
                timerCallback();
            }
        }
    }
    public void NextRound() {
        if (rounds.Count == 0) Debug.Log("Error: No Rounds");

        currentRound = rounds[Random.Range(0, rounds.Count)];
        currentRound.StartRound();
        UIManager.instance.UpdateRoundUI(currentRound);
    }

    public void SetTimer(float time, Callback c) {
        timer = time;
        timerCallback = c;
    }
}

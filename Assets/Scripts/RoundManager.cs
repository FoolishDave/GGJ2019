using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    public static RoundManager instance;
    private List<RoundAbstract> rounds;
    private RoundAbstract currentRound;
    [HideInInspector] public static RoundSettings settings;

    public delegate void Callback();
    public Callback timerCallback;

    private float timer;

    public delegate void PlayerEvent(int f);
    public event PlayerEvent onPlayerDeath;

    [HideInInspector] public List<FriendController> playersToRespawn;
    

    void Awake() {
        instance = this;
        rounds = new List<RoundAbstract>(GetComponentsInChildren<RoundAbstract>());
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

    public void TriggerPlayerDeath(int id) {
        if (onPlayerDeath != null) onPlayerDeath(id);
    }

    public void NextRound() {
        if (rounds.Count == 0) Debug.Log("Error: No Rounds");

        foreach (FriendController f in playersToRespawn) {
            //f.Respawn();
        }

        currentRound = rounds[Random.Range(0, rounds.Count)];
        settings = currentRound.settings;
        currentRound.StartRound();
        UIManager.instance.UpdateRoundUI(currentRound);
        
    }

    public void SetTimer(float time, Callback c) {
        timer = time;
        timerCallback = c;
    }
}

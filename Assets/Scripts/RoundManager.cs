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

    public float baseBomb, baseBlock;
    public UnityEngine.Coroutine bombSpawn, blockSpawn;
    public GameObject bombPrefab, blockPrefab;
    

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

        if (GameManager.instance.CheckWin()) return;

        timer = 0f; // Cancels timer if still active
        UIManager.instance.UpdateTimerUI(0f);
        foreach (FriendController f in playersToRespawn) {
            f.Respawn();
        }
        playersToRespawn.Clear();

        RoundAbstract next = rounds.Count == 1 ? rounds[0] : currentRound;
        while (currentRound == next && rounds.Count > 1) {
            next = rounds[Random.Range(0, rounds.Count)];
        }
        currentRound = next;
        settings = currentRound.settings;

        if (bombSpawn != null) StopCoroutine(bombSpawn);
        if (blockSpawn != null) StopCoroutine(blockSpawn);
        if (settings.bombSpawn) {
            bombSpawn = StartCoroutine(SpawnBomb());
        }
        if (settings.blockSpawn) {
            blockSpawn = StartCoroutine(SpawnBlock());
        }
        currentRound.StartRound();
        UIManager.instance.UpdateRoundUI(currentRound);
        
    }

    public void SetTimer(float time, Callback c) {
        timer = time;
        timerCallback = c;
    }

    IEnumerator SpawnBomb() {
        yield return new WaitForSeconds(baseBomb / settings.bombRate * Random.Range(.4f, .7f));
        while (true) {
            Quaternion rot = Quaternion.Euler(Random.Range(-12f,12f),Random.Range(-12f,12f),Random.Range(-12f,12f));
            Instantiate(bombPrefab, new Vector3(Random.Range(-2f,2f),4,Random.Range(-1f,1f)), rot);
            yield return new WaitForSeconds(baseBomb / settings.bombRate * Random.Range(.8f, 2f));
        }
        
    }

    IEnumerator SpawnBlock() {
        yield return new WaitForSeconds(baseBlock / settings.bombRate * Random.Range(.4f, .7f));
        while (true) {
            Quaternion rot = Quaternion.Euler(Random.Range(-12f,12f),Random.Range(-12f,12f),Random.Range(-12f,12f));
            Instantiate(blockPrefab, new Vector3(Random.Range(-2f,2f),4,Random.Range(-1f,1f)), rot);
            yield return new WaitForSeconds(baseBlock / settings.blockRate * Random.Range(.8f, 2f));
        }
        
    }
}

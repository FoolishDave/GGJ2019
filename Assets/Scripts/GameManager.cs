using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static List<FriendController> players;
    public static List<int> playerIDs;
    public static Dictionary<int, int> playerScores;

    void Awake() {
        instance = this;
    }

    void Start() {
        StartGame();
    }

    public void StartGame() {
        players = new List<FriendController>();
        playerIDs = new List<int>();
        playerScores = new Dictionary<int, int>();
        
        foreach (GameObject f in GameObject.FindGameObjectsWithTag("Player")) { // Sorry in advance
            FriendController c = f.GetComponent<FriendController>();
            players.Add(c);
            playerIDs.Add(c.playerId);
            playerScores.Add(c.playerId, 0);
        }

        UIManager.instance.CreateScoreUI();
        RoundManager.instance.NextRound();
    }

    public static void ChangeScore(int f, int score) {
        playerScores[f] += score;
        UIManager.instance.UpdateScoreUI();
    }
}

using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    //public static List<FriendController> players;
    public static Dictionary<int, int> playerScores;

    public bool gameRunning;

    void Awake() {
        instance = this;
    }

    void Start() {
        PlayerManager.Instance.PlayerJoined += PlayerJoined;
        PlayerManager.Instance.PlayerLeft += PlayerLeft;
    }

    public void StartGame() {
        gameRunning = true;
        playerScores = new Dictionary<int, int>();
        for (int i = 0; i < PlayerManager.Instance.Players.Length; i++) {
            playerScores.Add(i, 0);
            //players.Add(PlayerManager.Instance.PlayerObjects[i].GetComponent<FriendController>());
        }
        UIManager.instance.CreateScoreUI();
        RoundManager.instance.NextRound();
    }

    public static void ChangeScore(int f, int score) {
        playerScores[f] += score;
        UIManager.instance.UpdateScoreUI();
    }

    public static List<int> CreateIndexList() {
        List<int> l = new List<int>();
        for(int i = 0; i < PlayerManager.Instance.Players.Length; i++) {
            if (PlayerManager.Instance.Players != null) {
                l.Add(i);
            }
        }
        return l;
    }

    private void PlayerJoined(object sender, PlayerJoinedArgs args) {
        if (gameRunning) {
            playerScores.Add(args.playerId, 0);
            UIManager.instance.UpdateScoreUI();
        }
    }

    private void PlayerLeft(object sender, PlayerLeftArgs args) {
        if (gameRunning) { 
            playerScores.Remove(args.playerId);
            UIManager.instance.UpdateScoreUI();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soccer : RoundAbstract
{
    private List<int> leftTeamPlayers;
    private List<int> rightTeamPlayers;
    public GameObject wall;
    public GameObject ball;
    public Transform ballSpawnPoint;

    override public void StartRound() {
        List<int> players = GameManager.CreateIndexList();
        leftTeamPlayers = new List<int>();
        rightTeamPlayers = new List<int>();
        while(players.Count > 0) {
            int i = Random.Range(0, players.Count);
            leftTeamPlayers.Add(players[i]);
            players.RemoveAt(i);
            if (players.Count > 0) {
                i = Random.Range(0, players.Count);
                rightTeamPlayers.Add(players[i]);
                players.RemoveAt(i);
            }
        }
        ball.SetActive(true);
        wall.SetActive(true);
        ball.transform.position = ballSpawnPoint.position;

        desc = "";
        foreach(int i in leftTeamPlayers) {
            desc += "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerManager.Instance.playerColors[i]) + ">P" + (i+1) + "</color> ";
        }
        desc += "VS ";
        foreach(int i in rightTeamPlayers) {
            desc += "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerManager.Instance.playerColors[i]) + ">P" + (i+1) + "</color> ";
        }
        desc = desc.Trim();
    }

    public void Goal(bool left) {
        if (left) {
            foreach(int i in rightTeamPlayers) {
                GameManager.ChangeScore(i, 10);
            }
        } else {
            foreach(int i in leftTeamPlayers) {
                GameManager.ChangeScore(i, 10);
            }
        }
        EndRound();
    }

    override public void EndRound() {
        ball.SetActive(false);
        wall.SetActive(false);
        RoundManager.instance.NextRound();
    }
}

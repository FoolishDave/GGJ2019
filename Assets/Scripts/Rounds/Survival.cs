using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : RoundAbstract {

    private List<int> alivePlayers;

    override public void StartRound() {
        RoundManager.instance.SetTimer(10f, EndRound);
        RoundManager.instance.onPlayerDeath += OnPlayerDeath;
        alivePlayers = new List<int>(GameManager.playerIDs);
    }

    public void OnPlayerDeath(int f) {
        alivePlayers.Remove(f);
    }

    override public void EndRound() {
        RoundManager.instance.onPlayerDeath -= OnPlayerDeath;
        foreach (int player in alivePlayers) {
            GameManager.ChangeScore(player, 10);
        }
        RoundManager.instance.NextRound();
    }
}

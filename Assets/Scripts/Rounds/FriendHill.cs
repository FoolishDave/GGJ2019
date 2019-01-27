using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendHill : RoundAbstract {

    private List<int> alivePlayers;

    override public void StartRound() {
        RoundManager.instance.SetTimer(10f, EndRound);
        RoundManager.instance.onPlayerDeath += OnPlayerDeath;
        alivePlayers = GameManager.CreateIndexList();
    }

    public void OnPlayerDeath(int f) {
    }

    override public void EndRound() {
        RoundManager.instance.onPlayerDeath -= OnPlayerDeath;
        RoundManager.instance.NextRound();
    }
}

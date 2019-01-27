using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.GetComponent<RespawnOnFall>() != null) {
            other.GetComponent<RespawnOnFall>().Respawn();
            return;
        }
        if (other.tag == "Player") {
            FriendController f = other.GetComponent<FriendController>();
            RoundManager.instance.TriggerPlayerDeath(f.playerNum);
            if (!GameManager.instance.gameRunning) {
                f.Respawn();
            } else if (RoundManager.settings.playerRespawn) {
                f.Respawn();
            } else {
                RoundManager.instance.playersToRespawn.Add(f);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            FriendController f = other.GetComponent<FriendController>();
            RoundManager.instance.TriggerPlayerDeath(f.playerNum);
            if (RoundManager.settings.playerRespawn) {
                f.Respawn();
            } else {
                RoundManager.instance.playersToRespawn.Add(f);
            }
        }
    }
}

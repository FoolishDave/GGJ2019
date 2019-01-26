using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerObj;

    // Update is called once per frame
    void Update()
    {
        foreach (Player player in ReInput.players.AllPlayers)
        {
            if (player.GetAnyButton() && !player.isPlaying)
            {
                GameObject newPlayer = Instantiate(playerObj);
                newPlayer.GetComponent<FriendController>().playerId = player.id;
            }
        }
    }
}

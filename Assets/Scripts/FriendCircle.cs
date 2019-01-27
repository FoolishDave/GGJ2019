using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendCircle : MonoBehaviour
{
    public List<int> players = new List<int>();

    public void Clear() {
        players.Clear();
    }
    
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            players.Add(other.GetComponent<FriendController>().playerNum);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            players.Remove(other.GetComponent<FriendController>().playerNum);
        }
    }

    public List<int> GetPlayers() {
        return players;
    }
}

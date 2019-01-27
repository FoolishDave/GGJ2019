using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdPotato : RoundAbstract
{
    public GameObject potato;
    public Transform potatoSpawn;

    override public void StartRound() {
        potato.transform.position = potatoSpawn.position;
        RoundManager.instance.SetTimer(20, EndRound);
        potato.SetActive(true);
        StartCoroutine(AwardPoints());
    }

    IEnumerator AwardPoints() {
        for (int i = 0; i < 20; i++) {
            if (potato.transform.parent != null && potato.transform.parent.parent != null) {
                FriendController f = potato.transform.parent.parent.GetComponent<FriendController>();
                if (f != null) {
                    GameManager.ChangeScore(f.playerNum, 1);
                }
            }
            
            yield return new WaitForSeconds(1f);
        }
    }


    override public void EndRound() {
        potato.SetActive(false);
        potato.transform.SetParent(potatoSpawn);
        RoundManager.instance.NextRound();
    }
}

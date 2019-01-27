using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendHill : RoundAbstract {

    public FriendCircle circle;
    public GameObject stuff;

    override public void StartRound() {
        RoundManager.instance.SetTimer(20f, EndRound);
        circle.Clear();
        stuff.SetActive(true);
        StartCoroutine(AwardPoints());

    }

    IEnumerator AwardPoints() {
        for (int i = 0; i < 20; i++) {
            foreach(int p in circle.GetPlayers()) {
                GameManager.ChangeScore(p, 1);
            }
            yield return new WaitForSeconds(1f);
        }
        FriendCircle c;
        if ((c=GetComponent<FriendCircle>())!=null) {
            c.GetPlayers();
        }
        
    }



    override public void EndRound() {
        stuff.SetActive(false);
        RoundManager.instance.NextRound();
    }
}

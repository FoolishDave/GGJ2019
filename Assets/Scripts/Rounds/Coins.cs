using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : RoundAbstract
{
    public GameObject coinPrefab;
    override public void StartRound() {
        RoundManager.instance.SetTimer(25f, EndRound);
        StartCoroutine(CoinSpawn());
    }

    IEnumerator CoinSpawn() {
        for (int i = 1; i < 15; i++) {
            Instantiate(coinPrefab, new Vector3(Random.Range(-2f,2f),4,Random.Range(-1f,1f)), Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
        }
        
    }

    override public void EndRound() {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Coin")) {
            Destroy(g);
        }
        RoundManager.instance.NextRound();
    }
}

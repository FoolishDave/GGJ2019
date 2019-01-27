using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingItems : RoundAbstract
{
    public List<GameObject> items = new List<GameObject>();
    public float spawnRate = 2f;
    private List<int> players;
    private int[] startingPoints;
    private Coroutine pointCoroutine;
    private Coroutine spawnCoroutine;

    override public void StartRound() {
        players = GameManager.CreateIndexList();
        RoundManager.instance.SetTimer(30, EndRound);
        pointCoroutine = StartCoroutine(AwardPoints());
        spawnCoroutine = StartCoroutine(ItemSpawner());
    }

    IEnumerator AwardPoints() {
        while (true) {
            foreach (int p in players) {
                GameManager.ChangeScore(p, 1);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator ItemSpawner() {
        while (true) {
            GameObject obj = Instantiate(items[Random.Range(0, items.Count)]);
            obj.transform.parent = transform;
            obj.transform.position = new Vector3(Random.Range(-3.1f, 1.4f), 8f, Random.Range(-1.68f, 1.5f));
            obj.transform.eulerAngles = new Vector3(0, 0, Random.Range(-45, 45));
            yield return new WaitForSeconds(spawnRate + Random.Range(-1.5f, 1f));
        }
    }


    override public void EndRound() {
        StopCoroutine(pointCoroutine);
        StopCoroutine(spawnCoroutine);
        RoundManager.instance.NextRound();
    }
}

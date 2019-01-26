using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuTriggerButton : MonoBehaviour
{
    public bool requireAllPlayers;
    public float timeToActivate;

    public UnityEvent OnActivate;

    private float timer;
    private bool invoked;
    private int playersStanding = 0;

    private void OnEnable() {
        timer = timeToActivate;
    }

    void Update()
    {
        int playersReqired = requireAllPlayers ? PlayerManager.Instance.NumPlayers : 1;
        if (playersStanding >= playersReqired) {
            timer -= Time.deltaTime;
        } else {
            timer = timeToActivate;
            invoked = false;
        }

        if (timer <= 0f && !invoked) {
            OnActivate.Invoke();
            invoked = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playersStanding++;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            playersStanding--;
        }
    }
}

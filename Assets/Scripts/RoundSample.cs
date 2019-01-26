using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSample : RoundAbstract {

    override public void StartRound() {
        RoundManager.instance.SetTimer(10f, EndRound);
    }
}

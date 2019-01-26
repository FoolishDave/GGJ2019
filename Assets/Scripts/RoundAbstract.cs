using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoundAbstract : MonoBehaviour {
    public string title;
    public string desc;
    public abstract void StartRound();
    public virtual void EndRound() {
        RoundManager.instance.NextRound();
    }
}

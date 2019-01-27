using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemAbstract : MonoBehaviour
{
    public int playerId;
    public abstract void UseItem();
    public virtual void PickupItem(int id) {
        playerId = id;
    }
}

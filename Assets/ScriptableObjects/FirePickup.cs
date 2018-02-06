using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="custom/pickup/fire")]
public class FirePickup : PlayerPickup {

    [Tooltip("Buff duration in seconds."), Range(1f,15f)]
    public float buffTime;

    public override void Apply(PlayerElementHolder playerElementHolder){
        //SlotElement always needs to be implemented
        playerElementHolder.SlotElement(element);

    }

}
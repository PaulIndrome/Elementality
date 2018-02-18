using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="custom/pickup/fire")]
public class FirePickup : PlayerPickup {

    public float maxSpacingDistance, maxSpacingTime, fireBurnTime;
    [Tooltip("The fire passive will always spawn at least this amount of fires")]	
    public int effectAmount;

    public override void Apply(PlayerElementHolder playerElementHolder){
        base.Apply(playerElementHolder);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="custom/pickup/earth")]
public class EarthPickup : PlayerPickup {

    [Tooltip("Does this pickup give full invulnerability over the buff's duration?")]
    public bool fullInvulnerability;
    
    [Tooltip("Buff time in seconds")]
    public float buffTime;


    public override void Apply(PlayerElementHolder playerElementHolder){
        base.Apply(playerElementHolder);
    }

}
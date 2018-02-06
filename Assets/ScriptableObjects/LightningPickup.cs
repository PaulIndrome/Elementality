using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "custom/pickup/lightning")]
public class LightningPickup : PlayerPickup
{

    [Tooltip("Buff duration in seconds."), Range(1f, 15f)]
    public float buffTime;

    public override void Apply(PlayerElement playerElement)
    {
        //playerElement.LightningPickup(buffTime, playerElement.playerNum);

        //if(LightningBuffHandler.lightningEffectEvent != null)
        //    LightningBuffHandler.lightningEffectEvent(buffTime, elementHolder.playerNum);
        PlayerElement.lightningBuffCheck = 0;
        PlayerElement.lightningBuffRunningEvent();
		PlayerElement.lightningBuffEvent(playerElement.playerNum, true);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "custom/pickup/lightning")]
public class LightningPickup : PlayerPickup
{

    [Tooltip("Buff duration in seconds."), Range(1f, 15f)]
    public float buffTime;

    public override void Apply(PlayerElementHolder playerElementHolder)
    {
        playerElementHolder.SlotElement(element);

        PlayerLightningEffect.lightningBuffCheck = 0;
        PlayerLightningEffect.lightningBuffRunningEvent();
		PlayerLightningEffect.lightningBuffEvent(playerElementHolder.playerNum, true);
    }

}
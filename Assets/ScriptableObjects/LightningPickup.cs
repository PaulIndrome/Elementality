using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "custom/pickup/lightning")]
public class LightningPickup : PlayerPickup
{
    [Range(0.25f, 0.75f)]
    public float movementSpeedReduction, dodgeDistanceReduction;

}
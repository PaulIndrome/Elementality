using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="custom/pickup/health")]
public class HealthPickup : PlayerPickup {

   [Tooltip("Positive amount heals the player, negative amount damages the player"), Range(-3,3)]
   public int healthImpact;

}
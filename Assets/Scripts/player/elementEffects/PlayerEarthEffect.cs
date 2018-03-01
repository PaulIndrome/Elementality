using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEarthEffect : PlayerElementEffect {

	
	EarthPickup elementPickup;

	[Header("(public) element speficic variables")]
	public GameObject wallObject;

	public override void SlotPickup(PlayerPickup pickup){
		elementPickup = pickup as EarthPickup;
	}
	
	public override void CastDefense(){
		if(defenseEffect != null)
			StopCoroutine(defenseEffect);
		
		buffDuration = elementPickup.buffDuration;

		EarthDefenseWall wall = Instantiate(wallObject, transform.position, transform.rotation).GetComponent<EarthDefenseWall>();
		wall.SetupWall(playerNum, buffDuration);

		defenseEffect = StartCoroutine(ActivateDefenseShield());
	}
}

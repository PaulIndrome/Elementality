using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEarthEffect : PlayerElementEffect {

	public GameObject fireSpawnObject;
	bool fullInvulnerability;
	float buffDuration;
	Coroutine earthEffect; 

	public override void Activate(PlayerPickup pickup){
		EarthPickup ep = pickup as EarthPickup;

		if(earthEffect != null)
			StopCoroutine(earthEffect);

		fullInvulnerability = ep.fullInvulnerability;
		buffDuration = ep.buffTime;

		earthEffect = StartCoroutine(ActivateEarthShield());
	}
	public override void Interrupt(){
		if(earthEffect != null){
			StopCoroutine(earthEffect);
			earthEffect = null;
		}
	}
	
	IEnumerator ActivateEarthShield(){
		yield return null;

		earthEffect = null;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEarthEffect : PlayerElementEffect {

	public ParticleSystem earthShieldParticles;
	bool fullInvulnerability;
	float buffDuration;
	Coroutine earthEffect;
	Color playerColor;
	SphereCollider earthShieldCollider;

	void Start(){
		playerColor = GetComponentInParent<PlayerController>().playerColors[playerNum];
		earthShieldCollider = earthShieldParticles.gameObject.GetComponent<SphereCollider>();
		var main = earthShieldParticles.main;
		main.startColor = playerColor;
		earthShieldParticles.Stop();
		earthShieldCollider.enabled = false;
	}

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
			earthShieldParticles.Stop();
			earthShieldCollider.enabled = false;
		}
	}
	
	IEnumerator ActivateEarthShield(){
		earthShieldCollider.enabled = true;
		earthShieldParticles.Play();
		yield return new WaitForSeconds(buffDuration);
		earthShieldParticles.Stop();
		earthShieldCollider.enabled = false;
		earthEffect = null;
	}
}

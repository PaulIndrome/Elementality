using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupVisuals : MonoBehaviour {

	public Elements.Element element;
	public PlayerPickup pickup;
	public ParticleSystem idleParticles;

	[Tooltip("collectParticles should not loop")]
	public ParticleSystem collectParticles;

	MeshRenderer modelRenderer;
	SphereCollider sphereCollider;
	PowerUpCollect powerUpCollect;

	public void Setup () {
		modelRenderer = GetComponentInChildren<MeshRenderer>();
		sphereCollider = GetComponent<SphereCollider>();
		powerUpCollect = GetComponentInParent<PowerUpCollect>();
	}

	public void OnTriggerEnter(Collider collider){
		PlayerElementHolder playerElementHolder = collider.gameObject.GetComponent<PlayerElementHolder>();
		if(playerElementHolder != null){
			Collect();
			pickup.Apply(playerElementHolder);
		}
	}
	
	public void SetPickupActive(bool active){
		modelRenderer.enabled = active;
		sphereCollider.enabled = active;
		if(active) idleParticles.Play(true);
		else idleParticles.Stop(true);
	}

	public void Collect(){
		modelRenderer.enabled = false;
		sphereCollider.enabled = false;
		idleParticles.Stop(true);
		idleParticles.Clear(true);
		collectParticles.Play(true);
		powerUpCollect.PickupCollected();
	}

}

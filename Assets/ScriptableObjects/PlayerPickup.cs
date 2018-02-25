using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPickup : ScriptableObject {
	public Elements.Element element;
	public float buffDuration;
	public bool playActivationParticles = false;
	[Tooltip("The particleSystem on this gameObject should destroy the gameObject upon finishing")]
	public GameObject activationParticles;

	public virtual void Apply(PlayerElementHolder playerElementHolder){
		playerElementHolder.SlotElement(this);
		
		if(playActivationParticles && activationParticles != null && playerElementHolder.currentElement != Elements.Element.Earth) 
			Instantiate(activationParticles, playerElementHolder.transform.position, Quaternion.identity);
	}


}

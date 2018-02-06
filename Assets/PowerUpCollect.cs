using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollect : MonoBehaviour {

	public PlayerPickup pickupType;
	void OnTriggerEnter(Collider col){
		Debug.Log("PowerUp of type " + pickupType.element + " collected");
		PlayerElement playerElement = col.gameObject.GetComponent<PlayerElement>();
		if(playerElement != null){
			playerElement.SlotElement(pickupType.element);
			pickupType.Apply(playerElement);
		}
		
	}
}

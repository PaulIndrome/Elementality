using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollect : MonoBehaviour {

	[MinMaxRange(1,25)]
	public RangedFloat cycleTimeRange;
	public PlayerPickup[] pickupTypes;
	PlayerPickup currentPickupType;

	void Start(){
		StartCoroutine(CyclePickupType());
	}
	
	void OnTriggerEnter(Collider col){
		//Debug.Log("PowerUp of type " + currentPickupType.element + " collected");
		PlayerElementHolder playerElementHolder = col.gameObject.GetComponent<PlayerElementHolder>();
		if(playerElementHolder != null){
			currentPickupType.Apply(playerElementHolder);
		}
	}

	IEnumerator CyclePickupType(){
		PlayerPickup pickupTypeTemp;
		while(gameObject.activeSelf){
			pickupTypeTemp = pickupTypes[Random.Range(0,pickupTypes.Length)];
			while(pickupTypeTemp.element == currentPickupType.element)
				pickupTypeTemp = pickupTypes[Random.Range(0,pickupTypes.Length)];

			currentPickupType = pickupTypeTemp;
			yield return new WaitForSeconds(Random.Range(cycleTimeRange.minValue, cycleTimeRange.maxValue));
		}
	}

}

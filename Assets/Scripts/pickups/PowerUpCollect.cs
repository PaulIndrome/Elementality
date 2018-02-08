using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollect : MonoBehaviour {

	float timeToNextPickupType, lastPickupChangeTime;

	[MinMaxRange(1,25)]
	public RangedFloat respawnTimeRange;

	[MinMaxRange(1,25)]
	public RangedFloat cycleTimeRange;
	public PickupVisuals[] pickupVisuals;
	Elements.Element currentActiveElement;
	Coroutine cycle, respawn;

	public List<Elements.Element> excludedElements;

	void Start(){
		pickupVisuals = GetComponentsInChildren<PickupVisuals>();
		
		foreach(PickupVisuals pv in pickupVisuals){
			pv.Setup();
			pv.SetPickupActive(false);
		}

		ChangeActiveElement();

		cycle = StartCoroutine(CyclePickup());
	}
	
	public void ChangeActiveElement(){
		Elements.Element newElement = Elements.RandomElement();
		if(excludedElements.Count > 0){
			while(excludedElements.Contains(newElement))
				newElement = Elements.RandomElement();
		} else {
			while(newElement == currentActiveElement);
				newElement = Elements.RandomElement();
		}

		currentActiveElement = newElement;

		foreach(PickupVisuals pv in pickupVisuals){
			if(pv.element != currentActiveElement)
				pv.SetPickupActive(false);
			else 
				pv.SetPickupActive(true);
		}
	}

	public void PickupCollected(){
		StopCoroutine(cycle);
		cycle = null;
		respawn = StartCoroutine(RespawnTimer());
	}

	IEnumerator CyclePickup(){
		while(gameObject.activeSelf){
			yield return new WaitForSeconds(cycleTimeRange.Random());
			ChangeActiveElement();
		}
	}

	IEnumerator RespawnTimer(){
		yield return new WaitForSeconds(respawnTimeRange.Random());
		cycle = StartCoroutine(CyclePickup());
		respawn = null;
	}

}

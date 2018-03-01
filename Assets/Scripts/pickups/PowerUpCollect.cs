using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollect : MonoBehaviour {

	[MinMaxRange(5,25)]
	public RangedFloat respawnTimeRange;

	[MinMaxRange(5,25)]
	public RangedFloat cycleTimeRange;
	public PickupVisuals[] pickupVisuals;
	Elements.Element[] lastTwoElements;
	Coroutine cycle, respawn;
	[SerializeField] private ParticleSystem changeImminentPS;
	[SerializeField] private List<Elements.Element> excludedElements;

	void Start(){
		lastTwoElements = new Elements.Element[]{Elements.Element.None, Elements.Element.None};
		pickupVisuals = GetComponentsInChildren<PickupVisuals>();
		foreach(PickupVisuals pv in pickupVisuals){
			pv.Setup();
			pv.SetPickupActive(false);
		}
		cycle = StartCoroutine(CyclePickup());
	}
	
	public void ChangeActiveElement(){
		Elements.Element newElement = Elements.RandomElement();
		if(excludedElements.Count > 0){
			while(excludedElements.Contains(newElement)){
				newElement = Elements.RandomElement();
			}
		} else {
			while(newElement == lastTwoElements[0] || newElement == lastTwoElements[1]){
				newElement = Elements.RandomElement();
			}
		}

		lastTwoElements[0] = lastTwoElements[1];
		lastTwoElements[1] = newElement;

		foreach(PickupVisuals pv in pickupVisuals){
			if(pv.element != lastTwoElements[1])
				pv.SetPickupActive(false);
			else 
				pv.SetPickupActive(true);
		}
	}

	public void PickupCollected(){
		StopCoroutine(cycle);
		cycle = null;
		respawn = StartCoroutine(RespawnTimer());
		changeImminentPS.Stop();
	}

	IEnumerator CyclePickup(){
		if(respawn != null) {
			StopCoroutine(respawn);
			respawn = null;
		}
		ChangeActiveElement();
		while(gameObject.activeSelf){
			float cycleTime = cycleTimeRange.Random();
			yield return new WaitForSeconds(cycleTime - 4f);
			changeImminentPS.Play();
			yield return new WaitForSeconds(4f);
			ChangeActiveElement();
		}
	}

	IEnumerator RespawnTimer(){
		float respawnTime = respawnTimeRange.Random();
		yield return new WaitForSeconds(respawnTime - 4f);
		changeImminentPS.Play();
		yield return new WaitForSeconds(4f);
		cycle = StartCoroutine(CyclePickup());
		respawn = null;
	}

}

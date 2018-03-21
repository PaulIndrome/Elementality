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
	public Material pedestalMat;
	float loadAmount = 0f;

	void Start(){

		pedestalMat = GetComponent<MeshRenderer>().material;
		//pedestalMat.shader = Shader.Find("graphs/maskEmissionProgress");

		lastTwoElements = new Elements.Element[]{Elements.Element.None, Elements.Element.None};
		pickupVisuals = GetComponentsInChildren<PickupVisuals>();
		foreach(PickupVisuals pv in pickupVisuals){
			pv.Setup();
			pv.SetPickupActive(false);
		}
		cycle = StartCoroutine(CyclePickup2());
	}

	public void Update(){
		
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
		respawn = StartCoroutine(RespawnTimer2());
		changeImminentPS.Stop();
	}

	/*IEnumerator CyclePickup(){
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
	}*/

	IEnumerator CyclePickup2(){
		if(respawn != null) {
			StopCoroutine(respawn);
			respawn = null;
		}
		ChangeActiveElement();
		while(gameObject.activeSelf){
			loadAmount = 1f;
			float cycleTime = cycleTimeRange.Random();
			for(float t = 0; t <= cycleTime; t = t + Time.deltaTime){
				loadAmount = 1 - (t / cycleTime);
				pedestalMat.SetFloat("Vector1_A8323E03", loadAmount);
				yield return null;
			}
			ChangeActiveElement();
			yield return null;
		}

	}

	/*IEnumerator RespawnTimer(){
		float respawnTime = respawnTimeRange.Random();
		yield return new WaitForSeconds(respawnTime - 4f);
		changeImminentPS.Play();
		yield return new WaitForSeconds(4f);
		cycle = StartCoroutine(CyclePickup());
		respawn = null;
	}*/

	IEnumerator RespawnTimer2(){
		if(cycle != null) {
			StopCoroutine(cycle);
			cycle = null;
		}
		float respawnTime = respawnTimeRange.Random();
		loadAmount = 0f;
		for(float t = 0; t <= respawnTime; t = t + Time.deltaTime){
			loadAmount = t / respawnTime;
			pedestalMat.SetFloat("Vector1_A8323E03", loadAmount);
			yield return null;
		}
		cycle = StartCoroutine(CyclePickup2());
		respawn = null;
	}

}

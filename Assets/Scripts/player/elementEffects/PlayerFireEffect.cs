using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireEffect : PlayerElementEffect {

	public GameObject fireSpawnObject;
	int effectAmount;
	float maxSpacingDistance, maxSpacingTime, fireBurnTime;
	Coroutine fireEffect; 

	public override void Activate(PlayerPickup pickup){
		FirePickup fp = pickup as FirePickup;

		if(fireEffect != null)
			StopCoroutine(fireEffect);

		effectAmount = fp.effectAmount;
		fireBurnTime = fp.fireBurnTime;
		maxSpacingDistance = fp.maxSpacingDistance;
		maxSpacingTime = fp.maxSpacingTime;

		fireEffect = StartCoroutine(SpawnFireSteps());
		
	}
	public override void Interrupt(){
		if(fireEffect != null){
			StopCoroutine(fireEffect);
			fireEffect = null;
		}
	}
	public void SpawnFire(Vector3 position){
		GameObject spawn = Instantiate(fireSpawnObject, position, Quaternion.identity);
		spawn.GetComponent<FireSpawnCollision>().Setup(playerNum, fireBurnTime);
	}
	IEnumerator SpawnFireSteps(){
		int spawned = 0;
		float timer = 0;
		Vector3 lastSpawnPosition, currentPlayerPosition;
		currentPlayerPosition = lastSpawnPosition = transform.position;
		while(spawned < effectAmount){
			while(Vector3.Distance(currentPlayerPosition, lastSpawnPosition) < maxSpacingDistance && timer < maxSpacingTime){
				currentPlayerPosition = transform.position;
				timer += Time.deltaTime;
				yield return null;
			}
			lastSpawnPosition = currentPlayerPosition;
			timer = 0;
			SpawnFire(lastSpawnPosition);
			spawned++;
			yield return null;
		}
		fireEffect = null;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireEffect : PlayerElementEffect {

	FirePickup elementPickup;
	
	int effectAmount;
	float maxSpacingDistance, maxSpacingTime, fireBurnTime;
	Coroutine fireStepsSpawning; 

	[Header("(public) element speficic variables")]
	public GameObject fireSpawnObject;
	
	public override void SlotPickup(PlayerPickup pickup){
		elementPickup = pickup as FirePickup;
	}

	public override void CastDefense(){
		if(defenseEffect != null)
			StopCoroutine(defenseEffect);
		if(fireStepsSpawning != null)
			StopCoroutine(fireStepsSpawning);
		
		buffDuration = elementPickup.buffDuration;
		effectAmount = elementPickup.effectAmount;
		fireBurnTime = elementPickup.fireBurnTime;
		maxSpacingDistance = elementPickup.maxSpacingDistance;
		maxSpacingTime = elementPickup.maxSpacingTime;

		defenseEffect = StartCoroutine(ActivateDefenseShield());
		fireStepsSpawning = StartCoroutine(SpawnFireSteps());

	}

	public override void Interrupt(){
		base.Interrupt();
		if(fireStepsSpawning != null){
			StopCoroutine(fireStepsSpawning);
			fireStepsSpawning = null;
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
		while(spawned < effectAmount && defenseEffect != null){
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
		fireStepsSpawning = null;
	}

	/*protected override IEnumerator ActivateDefenseShield(){
		defenseCollider.enabled = defenseSphere.enabled = true;
		defenseParticles.Play();
		yield return new WaitForSeconds(buffDuration);
		defenseParticles.Stop();
		defenseCollider.enabled = defenseSphere.enabled = false;
		defenseEffect = null;
		if(fireStepsSpawning != null){
			StopCoroutine(fireStepsSpawning);
			fireStepsSpawning = null;
		}
	}*/
}

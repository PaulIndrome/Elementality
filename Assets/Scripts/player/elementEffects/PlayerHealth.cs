using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : PlayerElementEffect {

	[Range(0,3)]
	[SerializeField] private int currentHealth;
	[SerializeField] private float playerRespawnTime;

	PlayerElementHolder playerElementHolder;
	Vector3 oldPosition;
	Coroutine playerTimeOut;

	void Start(){
		playerElementHolder = GetComponent<PlayerElementHolder>();
	}

	public override void Activate(PlayerPickup pickup){
		HealthPickup hp = pickup as HealthPickup;
		
		currentHealth = Mathf.Clamp(currentHealth += hp.healthImpact, 0, 3);
		EvaluateHealth();
	}

	public override void Interrupt(){
		if(playerTimeOut != null)
			StopCoroutine(playerTimeOut);
		Respawn();
	}

	public void EvaluateHealth(){
		if(currentHealth <= 0){
			playerTimeOut = StartCoroutine(PlayerTimeOut());
		}
	}

	public void Respawn(){
		transform.position = oldPosition + Vector3.up;

		foreach(PlayerAction pA in GetComponents<PlayerAction>()){
			pA.enabled = true;
		}

		currentHealth = 3;
	}

	public int GetCurrentHealth(){
		return currentHealth;
	}

	public void SetCurrentHealth(int to){
		currentHealth = Mathf.Clamp(to, 0, 3);
	}

	IEnumerator PlayerTimeOut(){
		playerElementHolder.currentElement = Elements.Element.None;
		oldPosition = transform.position;

		foreach(PlayerElementEffect pE in GetComponents<PlayerElementEffect>()){
			if(pE != this)
				pE.Interrupt();
		}

		foreach(PlayerAction pA in GetComponents<PlayerAction>()){
			if(pA != this)
				pA.enabled = false;
		}
		
		transform.position = new Vector3(0f, -100, 0f);

		yield return new WaitForSecondsRealtime(playerRespawnTime);

		Respawn();
		playerTimeOut = null;
	}


}

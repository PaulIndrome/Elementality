using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : PlayerElementEffect {

	public delegate void HealthChangeDelegate(int newHealthVal);
	public HealthChangeDelegate healthChangeEvent;

	[Range(0,3)]
	[SerializeField] private int healthCurrent;
	[SerializeField] private float playerRespawnTime;

	PlayerElementHolder playerElementHolder;
	Vector3 oldPosition;
	Coroutine playerTimeOut;

	public int currentHealth {
		get { return healthCurrent;}
		set {
			if(value < 0) return;
			//Debug.Log("a " + healthCurrent);
			healthCurrent = value;
			//Debug.Log("b " + healthCurrent);
			healthCurrent = Mathf.Clamp(healthCurrent, 0, 3);
			//Debug.Log("c " + healthCurrent);
			if(healthChangeEvent != null)
				healthChangeEvent(healthCurrent);
		}
	}

	void Start(){
		playerElementHolder = GetComponent<PlayerElementHolder>();
		healthChangeEvent += EvaluateHealth;
	}

	public override void Activate(PlayerPickup pickup){
		HealthPickup hp = pickup as HealthPickup;
		
		currentHealth += hp.healthImpact;
	}

	public override void Interrupt(){
		if(playerTimeOut != null)
			StopCoroutine(playerTimeOut);
		Respawn();
	}

	public void EvaluateHealth(int newVal){
		if(newVal <= 0){
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

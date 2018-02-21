using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile {

	public float reorientSpeed;
	public float reorientTime = 1.5f;
	public float flightSpeed;
	

	public void OnTriggerEnter(Collider col){
		//Debug.Log("Projectile from " + shotByPlayerNum + " to " + shotAtPlayerNum + " collided with " + col.gameObject.name);
		PlayerMovement pm = col.GetComponent<PlayerMovement>();
		if(pm != null){
			StopCoroutine(moveProjectile);
			Destroy(gameObject);
		} else {
			return;
		}
	}

	public override Coroutine StartProjectile(){
		return StartCoroutine(MoveProjectile());
	}
	
	IEnumerator MoveProjectile(){
		float timer = 0;
		while(!collided){
			if(currentTarget != null && timer < reorientTime){
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentTarget.position - transform.position), reorientSpeed * Time.deltaTime);
				timer += Time.deltaTime;
			}
			transform.position += transform.forward * flightSpeed * Time.deltaTime;
			yield return null;
		}
	}
}

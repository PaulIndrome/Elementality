using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFlight : MonoBehaviour {

	public float reorientSpeed;
	public float reorientTime = 1.5f;
	public float flightSpeed;
	

	bool collided = false;
	int shotByPlayerNum, shotAtPlayerNum;
	Transform currentTarget;
	Coroutine moveProjectile;

	public void OnTriggerEnter(Collider col){
		Debug.Log("Projectile from " + shotByPlayerNum + " to " + shotAtPlayerNum + " collided with " + col.gameObject.name);
		PlayerMovement pm = col.GetComponent<PlayerMovement>();
		if(pm != null){
			StopCoroutine(moveProjectile);
			Destroy(gameObject);
		} else {
			return;
		}
	}

	public void Shoot(int shotByPlayer, PlayerMovement playerMov){
		if(shotByPlayer == playerMov.playerNum) {
			Debug.LogError("Player " + shotByPlayer + " shooting at " + playerMov.playerNum);
		}
		collided = false;
		shotByPlayerNum = shotByPlayer;
		shotAtPlayerNum = playerMov.playerNum;
		currentTarget = playerMov.transform;
		moveProjectile = StartCoroutine(MoveProjectile());
	}

	IEnumerator MoveProjectile(){
		float timer = 0;
		while(timer < reorientTime && !collided){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentTarget.position - transform.position), reorientSpeed * Time.deltaTime);
			transform.position += transform.forward * flightSpeed * Time.deltaTime;
			timer += Time.deltaTime;
			yield return null;
		}
		while(!collided){
			transform.position += transform.forward * flightSpeed * Time.deltaTime;
			yield return null;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile {


	
	

	protected override IEnumerator MoveProjectile(){
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

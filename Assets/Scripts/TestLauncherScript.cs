using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLauncherScript : MonoBehaviour {

	//public int targetPlayerNum;
	public GameObject projectilePrefab;
	
	public PlayerMovement[] playerMovements;

	// Use this for initialization
	void Start () {
		StartCoroutine(FireProjectiles());
	}
	
	IEnumerator FireProjectiles(){
		int shootAtPlayer = 1;
		while(gameObject.activeSelf){
			Projectile p = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
			p.Shoot(shootAtPlayer, playerMovements[(shootAtPlayer + 1) % playerMovements.Length]);
			shootAtPlayer = (shootAtPlayer + 1) % playerMovements.Length;
			yield return new WaitForSeconds(1f);
		}
	}

}

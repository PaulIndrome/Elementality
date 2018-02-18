using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffense : PlayerAction {

	public delegate Transform OffenseRangeCheckDelegate(int pNum, Transform initiateTransform);
	public static OffenseRangeCheckDelegate offenseRangeCheckEvent;

	PlayerElementHolder playerElementHolder;
	[SerializeField] private GameObject fireProjectile;
	[SerializeField] private GameObject lightningProjectile;
	[SerializeField] private GameObject earthProjectile;
	[SerializeField] LayerMask playerLayer;
	[SerializeField] float meleeRange, rangedRange, rangedConeAngle;

	bool shotCoolingDown = false;

	// Use this for initialization
	void Start () {
		playerElementHolder = GetComponent<PlayerElementHolder>();
		offenseRangeCheckEvent += offenseRangeCheck;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Offensive" + playerNum) >= 0.5f && !shotCoolingDown){
			shotCoolingDown = true;
			StartCoroutine(StartShotCooldown());
			if( playerElementHolder.currentElement == Elements.Element.None){
				Debug.LogError("No active element");
				return;
			}
			else {
				CastOffense();
			}
		}
	}

	Transform offenseRangeCheck(int pNum, Transform initiateTransform){
		if(playerNum == pNum)
			return null;

		if(	Vector3.Distance(initiateTransform.position, transform.position) < rangedRange && 
			Vector3.Angle(initiateTransform.forward, transform.position - initiateTransform.position) <= rangedConeAngle){
			return transform;
		} else 
			return null;
	}

	public void CastOffense(){
		Debug.Log("Offensecast");
		switch(playerElementHolder.currentElement){
			case Elements.Element.Earth: 
				ShootProjectileAt(earthProjectile, CheckMeleeRange());
				break;
			case Elements.Element.Fire: 
				ShootProjectileAt(fireProjectile, CheckMeleeRange());
				break;
			case Elements.Element.Lightning: 
				ShootProjectileAt(lightningProjectile, CheckMeleeRange());
				break;
		}
	}

	PlayerMovement CheckMeleeRange(){
		//int oldLayer = gameObject.layer;
		//gameObject.layer = 2;
		Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRange, playerLayer, QueryTriggerInteraction.Ignore);
		//gameObject.layer = oldLayer;
		foreach(Collider c in colliders)
			Debug.Log("Collisions: " + c.gameObject.name);

		if (colliders.Length == 1){
			PlayerMovement target = colliders[0].GetComponent<PlayerMovement>();
			if(target.playerNum != playerNum)
				return target;
			else {
				Debug.LogError("Player trying to hit himself in melee");
				return null;
			} 
		} else if (colliders.Length >= 2) {
			PlayerMovement[] players = new PlayerMovement[colliders.Length];
			float[] dots = new float[colliders.Length];
			
			PlayerMovement bestTarget = players[0] = colliders[0].GetComponent<PlayerMovement>();
			Debug.Log(colliders[0].gameObject.name);
			dots[0] = Vector3.Dot(transform.position, bestTarget.transform.position);

			for(int i = 1; i < colliders.Length; i++){
				players[i] = colliders[i].GetComponent<PlayerMovement>();
				dots[i] = Vector3.Dot(transform.position, players[i].transform.position);
				if(dots[i] > dots[i-1]){
					bestTarget = players[i];
				}
			}
			if(bestTarget.playerNum != playerNum)
				return bestTarget;
			else {
				Debug.LogError("Player trying to hit himself in melee");
				return null;
			}
		} else {
			//if no targets in meleeRange, check if you can shoot a projectile
			return CheckRangedRange();
		}

		//Debug.LogError("Melee Range if-case was skipped");
		//return null;
	}

	PlayerMovement CheckRangedRange(){
		List<Transform> playersInCone = new List<Transform>();

		if(offenseRangeCheckEvent != null)
			playersInCone.Add(offenseRangeCheckEvent(playerNum, transform));
		
		if(playersInCone.Capacity <= 0){
			return null;
		} else if (playersInCone.Capacity == 1){
			PlayerMovement bestTarget =  playersInCone[0].GetComponent<PlayerMovement>();
			if(bestTarget.playerNum != playerNum)
				return bestTarget;
			else {
				Debug.LogError("Player trying to hit himself in range");
				return null;
			}
		} else {
			PlayerMovement[] players = new PlayerMovement[playersInCone.Capacity];
			float[] dots = new float[playersInCone.Capacity];
			
			PlayerMovement bestTarget = players[0] = playersInCone[0].GetComponent<PlayerMovement>();
			dots[0] = Vector3.Dot(transform.position, bestTarget.transform.position);

			for(int i = 1; i < playersInCone.Capacity; i++){
				players[i] = playersInCone[i].GetComponent<PlayerMovement>();
				dots[i] = Vector3.Dot(transform.position, players[i].transform.position);
				if(dots[i] > dots[i-1]){
					bestTarget = players[i];
				}
			}

			if(bestTarget.playerNum != playerNum)
				return bestTarget;
			else {
				Debug.LogError("Player trying to hit himself in range");
				return null;
			}
		}
		
		Debug.LogError("Ranged if-case was skipped");
		return null;
	}

	void ShootProjectileAt(GameObject projectile, PlayerMovement target){
		Projectile p = Instantiate(projectile, transform.position + transform.forward, transform.rotation).GetComponent<Projectile>();
		p.Shoot(playerNum, target);
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, meleeRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, rangedRange);
		Gizmos.color = Color.magenta;

		Vector3 coneDir = Quaternion.AngleAxis(-rangedConeAngle, Vector3.up) * transform.forward;

		Gizmos.DrawRay(transform.position, coneDir.normalized * rangedRange);

		coneDir = Quaternion.AngleAxis(rangedConeAngle, Vector3.up) * transform.forward;

		Gizmos.DrawRay(transform.position, coneDir.normalized * rangedRange);
	}

	IEnumerator StartShotCooldown(){
		yield return new WaitUntil(() => Input.GetAxis("Offensive" + playerNum) < 0.25f);
		shotCoolingDown = false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffense : PlayerAction {

	public delegate void OffenseRangeCheckDelegate(int pNum, float range, Transform initiateTransform, List<Transform> list);
	public static event OffenseRangeCheckDelegate offenseRangeCheckEvent, offenseMeleeCheckEvent;

	PlayerElementHolder playerElementHolder;
	[SerializeField] LayerMask playerLayer;
	[SerializeField] float meleeRange = 0, rangedRange = 0, rangedConeAngle = 0;

	PlayerElementEffect earthElementEffect;
	PlayerElementEffect fireElementEffect;
	PlayerElementEffect lightningElementEffect;

	Collider[] colliders;

	bool offenseCoolingDown = false;
	string offenseAxis;

	void Start () {
		earthElementEffect = GetComponent<PlayerEarthEffect>();
		fireElementEffect = GetComponent<PlayerFireEffect>();
		lightningElementEffect = GetComponent<PlayerLightningEffect>();
		
		playerElementHolder = GetComponent<PlayerElementHolder>();
		offenseRangeCheckEvent += OffenseRangeCheck;
		offenseMeleeCheckEvent += OffenseMeleeCheck;
	}

	public override void SetPlayerNum(int pNum){
		base.SetPlayerNum(pNum);
		offenseAxis = "Offensive" + playerNum;
	}

	void Update () {
		if((Input.GetAxis(offenseAxis) >= 0.5f || Input.GetAxis(offenseAxis) <= -0.5f) && !offenseCoolingDown){
			offenseCoolingDown = true;
			StartCoroutine(StartOffenseCooldown());
			if(playerElementHolder.currentElement != Elements.Element.None){
				CastOffense();
			} 
		}
	}
	public void CastOffense(){
		CheckMeleeRange();
	}
	void OffenseMeleeCheck(int pNum, float initiateCloseRange, Transform initiateTransform, List<Transform> list){
		if(playerNum == pNum)
			return;
		if(Vector3.Distance(initiateTransform.position, transform.position) < initiateCloseRange){
			list.Add(transform);
			return;
		} else {
			return;
		}
	}
	void OffenseRangeCheck(int pNum, float initiateLongRange, Transform initiateTransform, List<Transform> list){
		if(playerNum == pNum)
			return;
		if(	Vector3.Distance(initiateTransform.position, transform.position) < initiateLongRange && 
			Vector3.Angle(initiateTransform.forward, transform.position - initiateTransform.position) <= rangedConeAngle){
			list.Add(transform);
			return;
		} else 
			return;
	}
	void CheckMeleeRange(){
		List<Transform> playersInMeleeRange;
		if(offenseMeleeCheckEvent != null){
			playersInMeleeRange = new List<Transform>();
			offenseMeleeCheckEvent(playerNum, meleeRange, transform, playersInMeleeRange);
		} else {
			return;
		}
		if(playersInMeleeRange.Count > 0){
			PlayerMovement[] players = new PlayerMovement[playersInMeleeRange.Count];
			for(int i = 0; i < playersInMeleeRange.Count; i++){
				players[i] = playersInMeleeRange[i].GetComponent<PlayerMovement>();
			}
			HitPlayersMelee(players);
			return;
		} else {
			//if no targets in meleeRange, check if you can shoot a projectile
			CheckRangedRange();
			return;
		}
	}
	void CheckRangedRange(){
		List<Transform> playersInCone;
		if(offenseRangeCheckEvent != null){
			playersInCone = new List<Transform>();
			offenseRangeCheckEvent(playerNum, rangedRange, transform, playersInCone);
		} else {
			return;
		}
		if(playersInCone.Count < 1){
			ShootProjectileAt(null);
			return;
		} else {
			PlayerMovement bestTarget = null;
			PlayerMovement[] players = new PlayerMovement[playersInCone.Count];
			float[] dots = new float[playersInCone.Count];
			float[] distances = new float[playersInCone.Count];
			
			for(int i = 0; i < playersInCone.Count; i++){
				players[i] = playersInCone[i].GetComponent<PlayerMovement>();
				dots[i] = Vector3.Dot(transform.forward, players[i].transform.position - transform.position);
				distances[i] = Vector3.Distance(transform.position, players[i].transform.position);

				//we only check for lowest distance so the angle needs to be low enough that it doesn't feel weird
				if (players[i].playerNum != playerNum && i == 0)
					bestTarget = players[i];
				else if(players[i].playerNum != playerNum && i > 0 && (distances[i] < distances[i-1]))
					bestTarget = players[i];
				else 
					continue;
			}
			ShootProjectileAt(bestTarget);
			return;
		}
	}
	void HitPlayersMelee(PlayerMovement[] targets){
		switch(playerElementHolder.currentElement){
			case Elements.Element.Earth: 
				earthElementEffect.CastOffense(targets);
				break;
			case Elements.Element.Fire: 
				fireElementEffect.CastOffense(targets);
				break;
			case Elements.Element.Lightning: 
				lightningElementEffect.CastOffense(targets);
				break;
			default:
				return;
		}
	}
	void ShootProjectileAt(PlayerMovement target){
		switch(playerElementHolder.currentElement){
			case Elements.Element.Earth: 
				earthElementEffect.CastOffense(target);
				break;
			case Elements.Element.Fire: 
				fireElementEffect.CastOffense(target);
				break;
			case Elements.Element.Lightning: 
				lightningElementEffect.CastOffense(target);
				break;
			default:
				return;
		}
	}
	IEnumerator StartOffenseCooldown(){
		yield return new WaitUntil(() => (Input.GetAxis(offenseAxis) < 0.2f && Input.GetAxis(offenseAxis) > -0.2f));
		offenseCoolingDown = false;
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
}
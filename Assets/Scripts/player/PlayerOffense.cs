using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffense : PlayerAction {

	PlayerElementHolder playerElementHolder;
	[SerializeField] private GameObject fireProjectile;
	[SerializeField] private GameObject lightningProjectile;
	[SerializeField] private GameObject earthProjectile;
	[SerializeField] LayerMask playerLayer;
	[SerializeField] float meleeRange, rangedRange;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Offensive" + playerNum) && playerElementHolder.currentElement != Elements.Element.None){
			CastOffense();
		}
	}

	public void CastOffense(){
		switch(playerElementHolder.currentElement){
			case Elements.Element.Earth: //shoot earth projectile
			break;
			case Elements.Element.Fire: //shoot fire projectile
			break;
			case Elements.Element.Lightning: //shoot lightning projectile
			break;
		}
	}

	public PlayerMovement FindTarget(){
		Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRange, playerLayer, QueryTriggerInteraction.Ignore);
		float[] angles = new float[colliders.Length];
		return null;
	}
}

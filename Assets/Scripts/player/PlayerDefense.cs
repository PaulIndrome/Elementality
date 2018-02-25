using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : PlayerAction {

	PlayerElementHolder playerElementHolder;
	PlayerElementEffect earthElementEffect;
	PlayerElementEffect fireElementEffect;
	PlayerElementEffect lightningElementEffect;
	bool defenseCoolingDown = false;

	// Use this for initialization
	void Start () {
		earthElementEffect = GetComponent<PlayerEarthEffect>();
		fireElementEffect = GetComponent<PlayerFireEffect>();
		lightningElementEffect = GetComponent<PlayerLightningEffect>();
		
		playerElementHolder = GetComponent<PlayerElementHolder>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Defensive" + playerNum) && !defenseCoolingDown){
			defenseCoolingDown = true;
			StartCoroutine(StartDefenseCooldown());
			if(playerElementHolder.currentElement != Elements.Element.None){
				CastDefense();
			}
		}
	}

	public void CastDefense(){
		switch(playerElementHolder.currentElement){
			case Elements.Element.Earth: 
				earthElementEffect.CastDefense();
				break;
			case Elements.Element.Fire: 
				fireElementEffect.CastDefense();
				break;
			case Elements.Element.Lightning: 
				lightningElementEffect.CastDefense();
				break;
			default:
				return;
		}
	}

	IEnumerator StartDefenseCooldown(){
		yield return new WaitUntil(() => Input.GetButtonUp("Defensive" + playerNum));
		defenseCoolingDown = false;
	}
}

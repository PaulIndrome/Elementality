using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerElementEffect : PlayerAction {
	public Elements.Element element;

	//[SerializeField] protected PlayerPickup elementPickup;
	[SerializeField] protected ParticleSpawner meleeParticle, shotParticle;
	[SerializeField] protected GameObject elementProjectile, defenseObject;

	protected float buffDuration;
	protected PlayerElementHolder playerElementHolder;
	protected Color playerColor;
	protected ParticleSystem defenseParticles;
	protected SphereCollider defenseCollider;
	protected Coroutine defenseEffect;

	public virtual void Start(){
		playerElementHolder = GetComponent<PlayerElementHolder>();
		playerColor = GetComponent<PlayerController>().playerColors[playerNum];

		if(defenseObject != null){
			defenseParticles = defenseObject.GetComponent<ParticleSystem>();
			defenseCollider = defenseObject.GetComponent<SphereCollider>();
			
			var main = defenseParticles.main;
			main.startColor = playerColor;
			defenseParticles.Stop();
			defenseCollider.enabled = false;
			defenseObject.SetActive(true);
		}
	}

	public abstract void SlotPickup(PlayerPickup pickup);

	//public abstract void Interrupt();

	public virtual void CastOffense(PlayerMovement target){
		//play rangedParticle for "muzzleflash"
		Projectile p;
		p = Instantiate(elementProjectile, transform.position + transform.forward, transform.rotation).GetComponent<Projectile>();
		p.Shoot(playerNum, target);
		return;
	}

	public virtual void CastOffense(PlayerMovement[] targets){
		meleeParticle.SpawnParticleSystem(transform);
		Projectile p;
		foreach(PlayerMovement pm in targets){
			if(pm.playerNum != playerNum){
				p = Instantiate(elementProjectile, transform.position + transform.forward, transform.rotation).GetComponent<Projectile>();
				p.Shoot(playerNum, pm);
			}
		}
		return;
	}

	public abstract void CastDefense();

	public virtual void Interrupt(){
		if(defenseEffect != null){
			StopCoroutine(defenseEffect);
			defenseEffect = null;
			defenseParticles.Stop();
			defenseCollider.enabled = false;
		}
	}
	
	protected virtual IEnumerator ActivateDefenseShield(){
		defenseCollider.enabled = true;
		defenseParticles.Play();
		yield return new WaitForSeconds(buffDuration);
		defenseParticles.Stop();
		defenseCollider.enabled = false;
		defenseEffect = null;
	}
}

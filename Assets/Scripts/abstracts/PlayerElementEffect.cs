using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerElementEffect : PlayerAction {
	public Elements.Element element;

	[SerializeField] PlayerPickup elementPickup;
	[SerializeField] protected ParticleSpawner meleeParticle, rangedParticle;
	[SerializeField] GameObject elementProjectile;

	public abstract void Activate(PlayerPickup pickup);

	public abstract void Interrupt();

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

	public virtual void CastDefense(){
		Activate(elementPickup);
		return;
	}
}

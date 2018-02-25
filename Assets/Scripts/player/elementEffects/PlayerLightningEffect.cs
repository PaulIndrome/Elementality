using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightningEffect : PlayerElementEffect{
	
	//ParticleSystem.MainModule mainModule;
	//public Color[] colors;
	LightningPickup elementPickup;
	PlayerMovement playerMovement;
	PlayerDodge playerDodge;
	float originalMoveSpeed, originalDodgeSpeed, originalDodgeDistance;
	float buffTime;

	#region lightning variables
	public delegate void lightningBuffDelegate(int pNum, bool startEnd);
	public delegate void lightningBuffRunningDelegate();
	public static lightningBuffDelegate lightningBuffEvent;
	public static lightningBuffRunningDelegate lightningBuffRunningEvent;
	public static int lightningBuffCheck;
	bool lightningBuffActive = false;
	public static float lightningSpeedReduction, lightningDistanceReduction = 0.5f;

	[Header("(public) element speficic variables")]
	public GameObject speedUpParticles;
	Coroutine lightningBuffTimer;
	#endregion

	public override void Start(){
		base.Start();

		if(lightningSpeedReduction != 0.5f)
			lightningSpeedReduction = 0.5f;

		//mainModule = transform.GetComponent<ParticleSystem>().main;
		//mainModule.startColor = colors[0];
		//GetComponent<ParticleSystem>().Play(false);

		playerMovement = GetComponent<PlayerMovement>();
		originalMoveSpeed = playerMovement.m_movementSpeed;

		playerDodge = GetComponent<PlayerDodge>();
		originalDodgeSpeed = playerDodge.m_dodgeSpeed;
		originalDodgeDistance = playerDodge.m_dodgeDistance;
		
		lightningBuffEvent += LightningEventListener;
		lightningBuffRunningEvent += LightningBuffRunning;
	}

	public override void SlotPickup(PlayerPickup pickup){
		elementPickup = pickup as LightningPickup;
	}

	public override void Interrupt(){
		base.Interrupt();
		if(lightningBuffTimer != null){
			StopCoroutine(lightningBuffTimer);
			EndLightningBuff();
		}
	}

	public override void CastDefense(){
		buffTime = elementPickup.buffDuration;
		lightningSpeedReduction = elementPickup.movementSpeedReduction;
		lightningDistanceReduction = elementPickup.dodgeDistanceReduction;

		defenseEffect = StartCoroutine(ActivateDefenseShield());

		lightningBuffCheck = 0;
		lightningBuffRunningEvent();
		lightningBuffEvent(playerNum, true);
	}

	#region lightning implementation
	public void LightningBuffRunning(){
		lightningBuffCheck += lightningBuffActive ? 1 : 0;
	}
	public void LightningEventListener(int pNum, bool startEnd){
		switch(startEnd){
			case true: // if a buff has started
				if(pNum == playerMovement.playerNum){ // and I started it because I'm a badass!
					if(lightningBuffTimer != null || lightningBuffActive) // I start a new localbufftimer and keep my speed or get it back for thiiiiiiis many seconds!
						StopCoroutine(lightningBuffTimer);
					SlowDownPlayer(false);
					lightningBuffTimer = StartCoroutine(LightningBuffTimer(buffTime));
					speedUpParticles.SetActive(true);
					//mainModule.startColor = colors[1];
					return;
				} else {
					if(lightningBuffTimer == null || !lightningBuffActive){ // However, if someone else started it and I don't have a buff on me ...
						//mainModule.startColor = colors[2]; // ... my speed is reduced ;_;
						SlowDownPlayer(true);
						return;
					}
					else {// if I DO have a buff on me, I keep my speed, yay! =^.^=
						return;
					}
				}
			case false: // if a buff has ended
				if(lightningBuffCheck <= 0){ // and no more buffs are running anywhere.
					//mainModule.startColor = colors[0];
					SlowDownPlayer(false); // everyone gets back up to speed, whoooo!
					return;
				} else {
					if(pNum == playerMovement.playerNum){ // otherwise the player who's buff has ended
						//mainModule.startColor = colors[2];
						SlowDownPlayer(true); // sadly gets his speed reduced. What a pity. 
						return;
					} else {
						return; // Now, every other player that had nothing to do with the ending buff just keeps on truckin'.
					}
				}
		}
	}
	public void SlowDownPlayer(bool slowed){
		if(slowed){
			speedUpParticles.SetActive(false);
			playerMovement.m_movementSpeed = originalMoveSpeed * lightningSpeedReduction;
			playerDodge.m_dodgeSpeed = originalDodgeSpeed * lightningSpeedReduction;
			playerDodge.m_dodgeDistance = originalDodgeDistance * lightningDistanceReduction;
		} else {
			playerMovement.m_movementSpeed = originalMoveSpeed;
			playerDodge.m_dodgeSpeed = originalDodgeSpeed;
			playerDodge.m_dodgeDistance = originalDodgeDistance;
		}
	}
	public void EndLightningBuff(){
		speedUpParticles.SetActive(false);
		lightningBuffActive = false;
		lightningBuffTimer = null;
		lightningBuffCheck = 0;
		lightningBuffRunningEvent();
		lightningBuffEvent(playerMovement.playerNum, false);
	}
	IEnumerator LightningBuffTimer(float buffTime){
		lightningBuffActive = true;
		yield return new WaitForSeconds(buffTime);
		EndLightningBuff();
	}
	#endregion
}
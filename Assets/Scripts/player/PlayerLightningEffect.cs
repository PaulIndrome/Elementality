using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightningEffect : PlayerAction{
	
	ParticleSystem.MainModule mainModule;
	public Color[] colors;
	PlayerMovement playerMovement;
	float originalMoveSpeed;

	public void Start(){
		if(lightningMoveSpeedReduction != 0.5f)
			lightningMoveSpeedReduction = 0.5f;

		mainModule = transform.GetComponent<ParticleSystem>().main;
		mainModule.startColor = colors[0];
		GetComponent<ParticleSystem>().Play();

		this.playerMovement = GetComponent<PlayerMovement>();
		originalMoveSpeed = this.playerMovement.m_movementSpeed;
		
		lightningBuffEvent += LightningEventListener;
		lightningBuffRunningEvent += LightningBuffRunning;
	}

	#region lightning variables
	public delegate void lightningBuffDelegate(int pNum, bool startEnd);
	public delegate void lightningBuffRunningDelegate();
	public static lightningBuffDelegate lightningBuffEvent;
	public static lightningBuffRunningDelegate lightningBuffRunningEvent;
	public static int lightningBuffCheck;
	bool lightningBuffActive = false;
	public static float lightningMoveSpeedReduction = 0.5f;
	Coroutine lightningBuffTimer;
	#endregion

	#region lightning implementation
	public void LightningBuffRunning(){
		lightningBuffCheck += lightningBuffActive ? 1 : 0;
	}

	public void LightningEventListener(int pNum, bool startEnd){
		//Debug.Log("Player " + playerNum + " says the check amounts to " + lightningBuffCheck + " at " + Time.time);
		//Debug.Log("Player " + playerNum + " movement speed = " + playerMovement.m_movementSpeed + " at " + Time.time);
		switch(startEnd){
			// if a buff has started
			case true:
				// and I started it because I'm a badass!
				if(pNum == playerNum){
					// I start a new localbufftimer and keep my speed or get it back for thiiiiiiis many seconds!
					if(lightningBuffTimer != null || lightningBuffActive) StopCoroutine(lightningBuffTimer);
					playerMovement.m_movementSpeed = originalMoveSpeed;
					lightningBuffTimer = StartCoroutine(LightningBuffTimer(5.0f));
					mainModule.startColor = colors[1];
					return;
				} else {
					// However, if someone else started it and I don't have a buff on me ...
					if(lightningBuffTimer == null || !lightningBuffActive){
						// ... my speed is reduced ;_;
						mainModule.startColor = colors[2];
						playerMovement.m_movementSpeed = originalMoveSpeed * lightningMoveSpeedReduction;
						return;
					}
					// if I DO have a buff on me, I keep my speed, yay! =^.^=
					else {
						return;
					}
				}
			// if a buff has ended
			case false:
				//Debug.Log("Check is at " + lightningBuffCheck + " at " + Time.time);
				// and no more buffs are running anywhere.
				if(lightningBuffCheck <= 0){
					// everyone gets back up to speed, whoooo!
					mainModule.startColor = colors[0];
					playerMovement.m_movementSpeed = originalMoveSpeed;
					return;
				} else {
					// otherwise the player who's buff has ended
					if(pNum == playerNum){
						// sadly gets his speed reduced. What a pity. 
						mainModule.startColor = colors[2];
						playerMovement.m_movementSpeed = originalMoveSpeed * lightningMoveSpeedReduction;
						//Debug.Log("Check players for difference in speed now");
						//Debug.Break();
						return;
					} else {
						// Now, every other player that had nothing to do with the ending buff just keeps on truckin'.
						return;
					}
				}
		}
	}

	IEnumerator LightningBuffTimer(float buffTime){
		lightningBuffActive = true;
		yield return new WaitForSeconds(buffTime);
		//Debug.Log("LocalBuff of Player " + playerNum + " running out" + " at " + Time.time);
		lightningBuffActive = false;
		lightningBuffTimer = null;
		lightningBuffCheck = 0;
		lightningBuffRunningEvent();
		lightningBuffEvent(playerNum, false);
	}
	#endregion


}
